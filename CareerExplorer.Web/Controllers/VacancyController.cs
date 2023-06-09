﻿using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace CareerExplorer.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly IJobSeekerProfileRepository _jobSeekerRepositoy;
        private readonly IJobSeekerVacancyRepository _jobSeekerVacancyRepository;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<SkillsTag> _skillsTagRepository;
        private readonly IVacancyService _vacancyService;
        private readonly IRepository<Position> _positionsRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IRepository<City> _cityRepository;

        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IRepository<AppUser> appUserRepository, IVacancyService vacancyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _recruiterRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _jobSeekerRepositoy = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _jobSeekerVacancyRepository = (IJobSeekerVacancyRepository)_unitOfWork.GetRepository<JobSeekerVacancy>();
            _appUserRepository = appUserRepository;
            _skillsTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _vacancyService = vacancyService;
            _positionsRepository = _unitOfWork.GetRepository<Position>();
            _countryRepository = (ICountryRepository)_unitOfWork.GetRepository<Country>();
            _cityRepository = _unitOfWork.GetRepository<City>();
        }
        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, string tagIds = "", string types = "")
        {
            try
            {
                int[] tagIdsArray = _vacancyService.GetIdsFromString(tagIds);
                int[] typesArray = _vacancyService.GetIdsFromString(types);
                List<Vacancy> vacancies;
                Expression<Func<Vacancy, bool>> filter = null; 
                if(tagIdsArray == null && typesArray != null)
                {
                    filter = x => x.WorkType != null && typesArray.Contains((int)x.WorkType);
                }
                else if(tagIdsArray != null && typesArray == null)
                {
                    filter = x => x.Requirements.Any(x => tagIdsArray.Contains(x.Id));
                }
                else if(tagIdsArray != null && typesArray != null)
                {
                    filter = x => x.WorkType != null && typesArray.Contains((int)x.WorkType)
                            && x.Requirements.Any(x => tagIdsArray.Contains(x.Id));
                }
                vacancies = _vacanciesRepository.GetAvailablePaginatedAndFilteredVacancies(StaticData.GetAllVacanciesPageSize,
                    pageNumber, out int totalVacancies, filter).ToList();
                var vacanciesDto = _mapper.Map<List<VacancyDTO>>(vacancies);
                var tagsItems = _skillsTagRepository.GetAll().Select(tag => new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Title
                });

                ViewBag.Tags = tagsItems;
                var paginatedVacancies = PaginatedList<VacancyDTO>
                    .Create(vacanciesDto, pageNumber, StaticData.GetAllVacanciesPageSize, totalVacancies);

                return View(paginatedVacancies);
            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult CreatedVacancies()
        {
            try
            {
                var currentRecruiterId = _userManager.GetUserId(User);
                if (currentRecruiterId == null)
                    return NotFound();

                var vacancies = _vacanciesRepository.GetCreatedVacancies(currentRecruiterId).ToList();
                bool isProfileAccepted = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecruiterId).IsAccepted;
                ViewBag.IsAccepted = isProfileAccepted;
                var vacanciesDTO = _mapper.Map<List<VacancyDTO>>(vacancies);
                return View(vacanciesDTO);
            }
            catch { return BadRequest(); }
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Search(string search)
        {
            var positions = _positionsRepository.GetAll(x => x.Name.ToLower().Contains(search.ToLower()));
            return Json(positions);
        }
        [Authorize(Roles = UserRoles.Recruiter)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVacancyDTO vacancyDTO, string selectedSkills, string position)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vacancyDTO);
                }
                if (selectedSkills == null)
                    return View(vacancyDTO);
                string currentRecruiterId = _userManager.GetUserId(User);
                
                if (currentRecruiterId == null) return BadRequest();
                var vacancy = _mapper.Map<Vacancy>(vacancyDTO);
                string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
                if (!int.TryParse(position, out int positionIndex) || tags == null)
                {
                    return BadRequest();
                }
                await _vacancyService.CreateVacancy(tags, positionIndex, currentRecruiterId, vacancy);
                return RedirectToAction(nameof(CreatedVacancies));
            }
            catch { return BadRequest(); }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult Edit(int? id) 
        { 
            try
            {
                if (id == null)
                    return BadRequest();
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id, "Requirements,Position,Country,City");
                var vacancyDto = _mapper.Map<EditVacancyDTO>(vacancy);
                ViewBag.PositionTitle = vacancy.Position.Name;
                ViewBag.PositionId = vacancy.Position.Id;
                ViewBag.Requirements = vacancy.Requirements.Select(x => x.Title).ToList();
                return View(vacancyDto);
            }
            catch { return BadRequest(); }
             
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Recruiter)]
        public async Task<IActionResult> Edit(EditVacancyDTO vacancyDto, string selectedSkills, string position)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vacancyDto);
                }
                var currentVacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == vacancyDto.Id, "Requirements");
                currentVacancy.Description = vacancyDto.Description;
                currentVacancy.IsAvailable = vacancyDto.IsAvailable;
                var country = _countryRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CountryId);
                var city = _cityRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CityId);
                currentVacancy.Country = country;
                currentVacancy.City = city;
                currentVacancy.ExperienceYears= vacancyDto.ExperienceYears;
                currentVacancy.Salary = vacancyDto.Salary;
                currentVacancy.EnglishLevel= vacancyDto.EnglishLevel;
                currentVacancy.WorkType= vacancyDto.WorkType;
                string[] tags = JsonConvert.DeserializeObject<string[]>(selectedSkills);
                if (!int.TryParse(position, out int positionIndex))
                {
                    return BadRequest();
                }
                await _vacancyService.EditVacancy(tags, currentVacancy, positionIndex);
                return RedirectToAction(nameof(CreatedVacancies));
            }
            catch { return BadRequest(); }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
                if (vacancy == null)
                    return BadRequest();
                _vacanciesRepository.Remove(vacancy);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(CreatedVacancies));
            }
            catch { return BadRequest(); }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetVacancy(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();
                var vacancy = await _vacanciesRepository.GetVacancyAsync(id);
                vacancy.Views++;
                await _unitOfWork.SaveAsync();
                ViewBag.ApplicantsCount = _vacanciesRepository.CountApplicantsOnVacancy(id);
                var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);

                var currentUserId = _userManager.GetUserId(User);
                var currentAppUser = _appUserRepository.GetFirstOrDefault(x => x.Id == currentUserId);

                if (currentAppUser.RecruiterProfileId != null)
                    return View(vacancyDto);
                if(currentAppUser.AdminProfileId!= null)
                    return View(vacancyDto);

                var joobSeeker = _jobSeekerRepositoy.GetFirstOrDefault(x => x.UserId == currentUserId);
                var joobSeekerId = joobSeeker.Id;
                ViewBag.IsProfileAccepted = joobSeeker.IsAccepted;
                ViewBag.IsProfileFilled = joobSeeker.IsFilled;
                //if jobSeekerVacancy object exist then jobSeeker has already applied on vacancy
                var applied = _jobSeekerVacancyRepository.GetFirstOrDefault(x => x.VacancyId == id && x.JobSeekerId == joobSeekerId);
                if (applied != null)
                    vacancyDto.IsApplied = true;
                else
                    vacancyDto.IsApplied = false;
                return View(vacancyDto);
            }
            catch { return BadRequest(); }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult GetApplicants(int id)
        {
            try
            {
                var jobSeekers = _jobSeekerVacancyRepository.GetApplicantsForVacancy(id);
                var applicants = _mapper.Map<List<ApplicantDTO>>(jobSeekers);
                foreach (var applicant in applicants)
                    applicant.VacancyId = id;

                return View(applicants);
            }
            catch { return BadRequest(); }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult GetApplicant(int jobSeekerId, int vacancyId)
        {
            try
            {
                if (jobSeekerId == 0 || vacancyId == 0)
                    return BadRequest();
                var jobSeeker = _jobSeekerRepositoy.GetFirstOrDefault(x => x.Id == jobSeekerId, "AppUser,Skills,Country,City,DesiredPosition");
                if (jobSeeker == null)
                    return BadRequest();
                var applicant = _mapper.Map<ApplicantDTO>(jobSeeker);
                applicant.VacancyId = vacancyId;
                ViewBag.ReceiverId = jobSeeker.AppUser.Id;
                return View(applicant);
            }
            catch { return BadRequest(); }
            
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Recruiter)]
        public IActionResult GetCv(int jobSeekerId, int vacancyId)
        {
            try
            {
                if (jobSeekerId == 0 || vacancyId == 0)
                    return BadRequest();
                var jobSeekerVacancy = _jobSeekerVacancyRepository.GetFirstOrDefault(x => x.JobSeekerId == jobSeekerId && x.VacancyId == vacancyId);
                if (jobSeekerVacancy == null)
                {
                    return NotFound();
                }
                return File(jobSeekerVacancy.Cv, "application/pdf");
            }
            catch { return BadRequest(); }
            
        }
    }
}
