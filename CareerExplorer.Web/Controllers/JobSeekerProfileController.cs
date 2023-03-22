using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;

namespace CareerExplorer.Web.Controllers
{
    public class JobSeekerProfileController : Controller
    {
        private readonly IApplyOnVacancyService _applyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IApplyOnVacancyService applyService)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
            _applyService= applyService;
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            var jobseekRep = _unitOfWork.GetJobSeekerRepository();
            var currentUserId = _userManager.GetUserId(User);
            var userProfile = jobseekRep.GetFirstOrDefault(x => x.UserId == currentUserId);
            return View(userProfile);
        }
        [HttpPost] 
        public IActionResult GetProfile(JobSeeker jobSeeker)
        {
            var jobseekRep = _unitOfWork.GetJobSeekerRepository();
            jobseekRep.Update(jobSeeker);
            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(GetProfile));
        }
        [HttpPost]
        public async Task<ActionResult> Apply(IFormFile file, string message)
        {

            return View();
        }
    }
}
