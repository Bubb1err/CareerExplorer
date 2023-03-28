using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.AccountManagement;
using System.IO.Compression;

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
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IApplyOnVacancyService applyService)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
            _applyService= applyService;
            _jobSeekerRepository = _unitOfWork.GetJobSeekerRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetJobSeekerVacancyRepository();
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var userProfile = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == currentUserId);
                if (userProfile == null)
                {
                    return NotFound();
                }
                return View(userProfile);
            }
            catch { return BadRequest(); }
            
        }
        [HttpPost] 
        public async Task<IActionResult> GetProfile(JobSeeker jobSeeker)
        {
            try
            {
                if (jobSeeker == null)
                    return NotFound();
                _jobSeekerRepository.Update(jobSeeker);
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
