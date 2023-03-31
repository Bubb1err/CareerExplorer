using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.DirectoryServices.AccountManagement;
using System.IO.Compression;
using System.Linq;

namespace CareerExplorer.Web.Controllers
{
    [Authorize(Roles = UserRoles.JobSeeker)]
    public class JobSeekerProfileController : Controller
    {
        private readonly IApplyOnVacancyService _applyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IJobSeekerVacancyRepository _jobSeekerVacancyRepository;
        private readonly IRepository<SkillsTag> _skillsTagRepository;
        private readonly IMapper _mapper;
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IApplyOnVacancyService applyService, IMapper mapper)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
            _applyService= applyService;
            _skillsTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _jobSeekerRepository = _unitOfWork.GetJobSeekerRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetJobSeekerVacancyRepository();
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var userProfile = _jobSeekerRepository.GetJobSeeker(currentUserId);
                if (userProfile == null)
                {
                    return NotFound();
                }
                var jobSeekerDto = _mapper.Map<JobSeekerDTO>(userProfile);
                return View(jobSeekerDto);
            }
            catch { return BadRequest(); }
            
        }
        [HttpPost] 
        public async Task<IActionResult> GetProfile(JobSeekerDTO jobSeekerDto, string selectedSkills)
        {
            try
            {
                
                string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
                if (!ModelState.IsValid)
                {
                    var skills = new List<SkillsTag>();
                    foreach(var tag in tags)
                    {
                        skills.Add(_skillsTagRepository.GetFirstOrDefault(x => x.Title == tag));
                    }
                    jobSeekerDto.Skills = skills;
                    return View(jobSeekerDto);
                }

                var currentUserId = _userManager.GetUserId(User);
                var userProfile = _jobSeekerRepository.GetJobSeeker(currentUserId);

                userProfile.Name = jobSeekerDto.Name;
                userProfile.Surname = jobSeekerDto.Surname;
                userProfile.Phone= jobSeekerDto.Phone;
                userProfile.Experience = jobSeekerDto.Experience;
                userProfile.GitHub= jobSeekerDto.GitHub;

                List<SkillsTag> skillsToAdd = new List<SkillsTag>();
                var existingSkillTags = userProfile.Skills.Select(s => s.Title);
                var tagsToRemove = existingSkillTags.Except(tags).ToList();
                for (int i = tagsToRemove.Count() - 1; i >= 0; i--)
                {
                    var skillTagToRemove = userProfile.Skills.FirstOrDefault(s => s.Title == tagsToRemove[i]);
                    userProfile.Skills.Remove(skillTagToRemove);
                }
                foreach (var skillTag in tags.Except(existingSkillTags))
                {
                    var newSkillTag = _skillsTagRepository.GetFirstOrDefault(s => s.Title == skillTag)
                        ?? new SkillsTag { Title = skillTag };

                    userProfile.Skills.Add(newSkillTag);
                }
                _jobSeekerRepository.Update(userProfile);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetProfile));
            }
            catch { return BadRequest(); }
            
        }
        
        [HttpPost]
        [Authorize(Roles = UserRoles.JobSeeker)]
        public async Task<IActionResult> Apply(IFormFile file, int vacancyId)
        {
            if (file == null || file.Length == 0 || vacancyId == 0)
                return BadRequest();
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (currentUserId == null)
                    return BadRequest();
                await _applyService.Apply(currentUserId, vacancyId, file);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return Ok();
            
        }
    }
}
