using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;

namespace CareerExplorer.Web.Controllers
{
    [Authorize(Roles = UserRoles.RoleJobSeeker)]
    public class JobSeekerProfileController : Controller
    {
        private readonly IApplyOnVacancyService _applyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        private readonly ICvPathsRepository _cvPathsRepository;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IRepository<JobSeekerVacancy> _jobSeekerVacancyRepository;
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IApplyOnVacancyService applyService)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
            _applyService= applyService;
            _jobSeekerRepository = _unitOfWork.GetJobSeekerRepository();
            _cvPathsRepository = _unitOfWork.GetCvPathsRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetRepository<JobSeekerVacancy>();
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            var currentUserId = _userManager.GetUserId(User);
            var userProfile = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == currentUserId);
            return View(userProfile);
        }
        [HttpPost] 
        public async Task<IActionResult> GetProfile(JobSeeker jobSeeker)
        {
            _jobSeekerRepository.Update(jobSeeker);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(GetProfile));
        }
        
        [HttpPost]
        public async Task<IActionResult> Apply(IFormFile file, int vacancyId)
        {
            if (vacancyId == 0)
                return BadRequest();

            //getting current user
            var currentUserId = _userManager.GetUserId(User);
            var jobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == currentUserId);
            int jobSeekerId = jobSeeker.Id;

            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CvStorage");

            //save cv to local folder with name <guid>-jobseekerid-vacancyid.<fileExtension> 
            var fileName = await _applyService.SaveCv(file, folderPath, jobSeekerId, vacancyId);

            //create CvPath entity
            var cvPath = _applyService.CreateCvPath(fileName, jobSeekerId, jobSeeker);
            _cvPathsRepository.Add(cvPath);

            jobSeeker.PathsToAppliedCvs.Add(cvPath);

            var vacancyApplied = _vacanciesRepository.GetFirstOrDefault( x => x.Id == vacancyId);

            //create JobSeekerVacancy entity
            var jobSeekVac = _applyService.CreateJobSeekerVacancy(jobSeekerId, jobSeeker, vacancyId, vacancyApplied);
            _jobSeekerVacancyRepository.Add(jobSeekVac);

            await _unitOfWork.SaveAsync();

            jobSeekVac = _jobSeekerVacancyRepository.GetFirstOrDefault(x => x.VacancyId == vacancyId && x.JobSeekerId== jobSeekerId);

            vacancyApplied.Applicants.Add(jobSeekVac);
            jobSeeker.VacanciesApplied.Add(jobSeekVac);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
