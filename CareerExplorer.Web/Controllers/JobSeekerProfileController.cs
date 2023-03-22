using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;

namespace CareerExplorer.Web.Controllers
{
    public class JobSeekerProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public JobSeekerProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager= userManager;
            _unitOfWork = unitOfWork;
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
    }
}
