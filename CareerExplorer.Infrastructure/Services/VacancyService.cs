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
        private readonly ICountryRepository _countryRepository;
        private readonly IRepository<City> _cityRepository;
        public VacancyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
            _positionsRepository = _unitOfWork.GetRepository<Position>();
            _skillsTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _recruiterRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _countryRepository = (ICountryRepository)_unitOfWork.GetRepository<Country>();
            _cityRepository = _unitOfWork.GetRepository<City>();
        }
        public int[]? GetIdsFromString(string ids)
        {
            if(ids.IsNullOrEmpty())
                return null;
            string[] tagIdsStr = ids.Split(',');
            int[] tagIdsArray = new int[tagIdsStr.Length];
            for (int i = 0; i < tagIdsStr.Length; i++)
            {
                if(int.TryParse(tagIdsStr[i], out int tagId))
                {
                    tagIdsArray[i] = tagId;
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
        public int[]? GetTypesFromString(string types)
        {
            if(types.IsNullOrEmpty()) 
                return null;
            string[] typesStr = types.Split(',');
            int[] typesArray = new int[typesStr.Length];
            for(int i = 0; i < typesStr.Length;i++)
            {
                if (int.TryParse(typesStr[i], out int type))
                {
                    typesArray[i] = type;
                }
                else
                {
                    break; 
                    throw new ArgumentException();
                }
            }
            if(typesArray.Length == 0)
                return null;
            return typesArray;
        }
        public async Task CreateVacancy(string selectedSkills, string position, string currentRecruiterId, Vacancy vacancy)
        {
            string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
            if (!int.TryParse(position, out int positionIndex))
            {
                throw new ArgumentException();
            }
            var positionSelected = _positionsRepository.GetFirstOrDefault(x => x.Id == positionIndex);
            var skills = _skillsTagRepository.GetAll(x => tags.Contains(x.Title)).ToList();
            //foreach (var tag in tags)
            //{
            //    skills.Add(_skillsTagRepository.GetFirstOrDefault(x => x.Title == tag));
            //}
            var country = _countryRepository.GetFirstOrDefault(x => x.Id == vacancy.CountryId);
            var city = _cityRepository.GetFirstOrDefault(x => x.Id == vacancy.CityId);
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
            var existingSkillTags = vacancy.Requirements.Select(s => s.Title);
            var tagsToRemove = vacancy.Requirements.Where(x => !tags.Contains(x.Title)).ToList();
            for (int i = tagsToRemove.Count()-1; i >= 0; i--)
            {
                vacancy.Requirements.Remove(tagsToRemove[i]);
            }
            var skillsToAdd = _skillsTagRepository.GetAll(x => tags.Except(existingSkillTags).Contains(x.Title)).ToList();
            for(int i = skillsToAdd.Count()-1; i >= 0;i--)
            {
                vacancy.Requirements.Add(skillsToAdd[i]);
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
