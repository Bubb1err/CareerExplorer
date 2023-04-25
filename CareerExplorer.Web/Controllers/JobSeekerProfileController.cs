using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CareerExplorer.Web.Controllers
{
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
        private readonly IAdminService _adminService;
        private readonly ICountryRepository _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Position> _positionRepository;
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IApplyOnVacancyService applyService, IMapper mapper, IAdminService adminService)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
            _applyService= applyService;
            _skillsTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _jobSeekerVacancyRepository = (IJobSeekerVacancyRepository)_unitOfWork.GetRepository<JobSeekerVacancy>();
            _mapper = mapper;
            _adminService = adminService;
            _countryRepository = (ICountryRepository)_unitOfWork.GetRepository<Country>();
            _cityRepository = _unitOfWork.GetRepository<City>();
            _positionRepository = _unitOfWork.GetRepository<Position>();
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.JobSeeker)]
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
        [Authorize(Roles = UserRoles.JobSeeker)]
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
                var country = _countryRepository.GetFirstOrDefault(x => x.Id == jobSeekerDto.CountryId);
                var city = _cityRepository.GetFirstOrDefault(x => x.Id == jobSeekerDto.CityId);
                var position = _positionRepository.GetFirstOrDefault(x => x.Id == jobSeekerDto.DesiredPositionId);

                userProfile.Name = jobSeekerDto.Name;
                userProfile.Surname = jobSeekerDto.Surname;
                userProfile.Phone= jobSeekerDto.Phone;
                userProfile.Experience = jobSeekerDto.Experience;
                userProfile.GitHub= jobSeekerDto.GitHub;
                userProfile.CountryId = jobSeekerDto.CountryId;
                userProfile.Country = country;
                userProfile.CityId = jobSeekerDto.CityId;
                userProfile.City = city;
                userProfile.DesiredPositionId= jobSeekerDto.DesiredPositionId;
                userProfile.DesiredPosition = position;
                userProfile.Salary = jobSeekerDto.Salary;
                userProfile.LinkedIn = jobSeekerDto.LinkedIn;
                userProfile.EnglishLevel = jobSeekerDto.EnglishLevel;
                userProfile.ExperienceYears = jobSeekerDto.ExperienceYears;
                
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
                userProfile.IsFilled = _adminService.IsJobSeekerProfileFilled(userProfile);
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
        [HttpGet]
        [Authorize]
        public IActionResult CountriesSearch(string? search)
        {
            var countries = _countryRepository.GetFirstCountries(search).ToList();
            return Json(countries);
        }
        [HttpGet]
        [Authorize]
        public IActionResult CitiesSearch(string? search, int countryId)
        {
            var cities = _countryRepository.GetFirstCitiesOfCountry(countryId, search).ToList();
            return Json(cities);
        }
        //[HttpGet]
        //public async Task<IActionResult> Start(string chatId)
        //{
        //    long chatIdToLong = long.Parse(chatId);

        //    var user = await _userManager.GetUserAsync(User);
        //    var userId = user.Id;
        //    var jobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == userId);
        //    jobSeeker.TgChatId= chatIdToLong;
        //    await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
