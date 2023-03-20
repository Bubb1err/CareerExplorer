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
            var currentUserId = _userManager.GetUserId(User);
            var userProfile = _unitOfWork.JobSeekerProfile.GetFirstOrDefault(x => x.UserId == currentUserId);
            return View(userProfile);
        }
        [HttpPost] 
        public IActionResult GetProfile(JobSeeker jobSeeker)
        {
            _unitOfWork.JobSeekerProfile.Update(jobSeeker);
            _unitOfWork.Save();
            return RedirectToAction("GetProfile");
        }
    }
}
