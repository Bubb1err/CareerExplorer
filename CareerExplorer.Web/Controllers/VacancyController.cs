﻿using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;

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
        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IRepository<AppUser> appUserRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            //_vacanciesRepository = _unitOfWork.GetRepository<Vacancy, VacanciesRepository>();
            _recruiterRepository = _unitOfWork.GetRecruiterRepository();
            _jobSeekerRepositoy = _unitOfWork.GetJobSeekerRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetJobSeekerVacancyRepository();
            _appUserRepository = appUserRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1)
        {
            try
            {
                const int pageSize = 1;
                var vacancies = _vacanciesRepository.GetAvailablePaginatedVacancies(pageSize, pageNumber);
                var totalVacancies = _vacanciesRepository.CountVacancies();

                var vacanciesDto = _mapper.Map<List<VacancyDTO>>(vacancies);
                foreach (var vacancyDto in vacanciesDto)
                {
                    var creator = _recruiterRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CreatorId);
                    if (creator == null)
                        return BadRequest();
                    var appUserCreator = _appUserRepository.GetFirstOrDefault(x => x.RecruiterProfileId == creator.Id);
                    if (appUserCreator == null)
                        return BadRequest();
                    var creatorEmail = await _userManager.GetEmailAsync(appUserCreator);
                    if (creatorEmail == null)
                        return BadRequest();
                    vacancyDto.CreatorNickName = creatorEmail;
                }

                var paginatedVacancies = PaginatedList<VacancyDTO>.Create(vacanciesDto, pageNumber, pageSize, totalVacancies);

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
        [Authorize(Roles = UserRoles.Recruiter)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditVacancyDTO vacancyDTO)
        {
            try
            {
                if (vacancyDTO == null)
                    return BadRequest(ModelState);
                var currentRecruiterId = _userManager.GetUserId(User);

                var creator = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecruiterId);
                var vacancy = _mapper.Map<Vacancy>(vacancyDTO);
                vacancy.CreatorId = creator.Id;
                vacancy.Creator = creator;
                await _vacanciesRepository.AddAsync(vacancy);

                await _unitOfWork.SaveAsync();
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
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
                var vacancyDto = _mapper.Map<CreateOrEditVacancyDTO>(vacancy);
                return View(vacancyDto);
            }
            catch { return BadRequest(); }
             
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Recruiter)]
        public async Task<IActionResult> Edit(CreateOrEditVacancyDTO vacancyDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vacancy = _mapper.Map<Vacancy>(vacancyDto);
                    _vacanciesRepository.Update(vacancy);
                    await _unitOfWork.SaveAsync();
                    return RedirectToAction(nameof(CreatedVacancies));
                }
                return View(vacancyDto);
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
        public async Task<IActionResult> GetVacancy(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
                var creator = _recruiterRepository.GetFirstOrDefault(x => x.Id == vacancy.CreatorId);

                var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);

                var appUserCreator = _appUserRepository.GetFirstOrDefault(x => x.RecruiterProfileId == creator.Id);
                var creatorEmail = await _userManager.GetEmailAsync(appUserCreator);
                if (creatorEmail == null)
                    return BadRequest();
                vacancyDto.CreatorNickName = creatorEmail;

                var currentUserId = _userManager.GetUserId(User);
                var currentAppUser = _appUserRepository.GetFirstOrDefault(x => x.Id == currentUserId);

                if (currentAppUser.RecruiterProfileId != null)
                    return View(vacancyDto);

                var joobSeekId = _jobSeekerRepositoy.GetFirstOrDefault(x => x.UserId == currentUserId).Id;

                //if jobSeekerVacancy object exist then jobSeeker has already applied on vacancy
                var applied = _jobSeekerVacancyRepository.GetFirstOrDefault(x => x.VacancyId == id && x.JobSeekerId == joobSeekId);
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
                var jobSeeker = _jobSeekerRepositoy.GetFirstOrDefault(x => x.Id == jobSeekerId);
                if (jobSeeker == null)
                    return BadRequest();
                var applicant = _mapper.Map<ApplicantDTO>(jobSeeker);
                applicant.VacancyId = vacancyId;
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
