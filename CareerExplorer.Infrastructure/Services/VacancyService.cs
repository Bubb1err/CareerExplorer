using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Infrastructure.Repository;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Position> _positionsRepository;
        private readonly IRepository<SkillsTag> _skillsTagRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly IVacanciesRepository _vacanciesRepository;
        public VacancyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
            _positionsRepository = _unitOfWork.GetRepository<Position>();
            _skillsTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _recruiterRepository = _unitOfWork.GetRecruiterRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
        }
        public int[]? GetIdsFromString(string ids)
        {
            if(ids.IsNullOrEmpty())
                return null;
            string[] tagIdsStr = ids.Split(',');
            int[] tagIdsArray = new int[tagIdsStr.Length];
            for (int i = 0; i < tagIdsStr.Length; i++)
            {
                if (char.IsDigit(char.Parse(tagIdsStr[i])))
                {
                    tagIdsArray[i] = int.Parse(tagIdsStr[i]);
                }
                else
                {
                    break;
                    throw new ArgumentException();   
                }
            }
            if (tagIdsArray.Length == 0)
                return null;
            return tagIdsArray;
        }
        public async Task CreateVacancy(string selectedSkills, string position, string currentRecruiterId, Vacancy vacancy)
        {
            string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
            if (!int.TryParse(position, out int positionIndex))
            {
                throw new ArgumentException();
            }
            var positionSelected = _positionsRepository.GetFirstOrDefault(x => x.Id == positionIndex);
            var skills = new List<SkillsTag>();
            foreach (var tag in tags)
            {
                skills.Add(_skillsTagRepository.GetFirstOrDefault(x => x.Title == tag));
            }
            var creator = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecruiterId);
            vacancy.Requirements = skills;
            vacancy.CreatorId = creator.Id;
            vacancy.Creator = creator;
            vacancy.CreatedDate = DateTime.Now;
            vacancy.Position = positionSelected;
            vacancy.PositionId = positionIndex;
            await _vacanciesRepository.AddAsync(vacancy);
            await _unitOfWork.SaveAsync();
        }
        public async Task EditVacancy(string selectedSkills, Vacancy vacancy, string position)
        {
            if(selectedSkills== null)
                throw new ArgumentNullException();
            string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
            if (!int.TryParse(position, out int positionIndex))
            {
                throw new ArgumentException();
            }
            if (vacancy.PositionId != positionIndex)
            {
                var positionSelected = _positionsRepository.GetFirstOrDefault(x => x.Id == positionIndex);
                vacancy.Position = positionSelected;
                vacancy.PositionId = positionIndex;
            }
            List<SkillsTag> skillsToAdd = new List<SkillsTag>();
            var existingSkillTags = vacancy.Requirements.Select(s => s.Title);
            var tagsToRemove = existingSkillTags.Except(tags).ToList();
            for (int i = tagsToRemove.Count() - 1; i >= 0; i--)
            {
                var skillTagToRemove = vacancy.Requirements.FirstOrDefault(s => s.Title == tagsToRemove[i]);
                vacancy.Requirements.Remove(skillTagToRemove);
            }
            foreach (var skillTag in tags.Except(existingSkillTags))
            {
                var newSkillTag = _skillsTagRepository.GetFirstOrDefault(s => s.Title == skillTag)
                    ?? new SkillsTag { Title = skillTag };

                vacancy.Requirements.Add(newSkillTag);
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
