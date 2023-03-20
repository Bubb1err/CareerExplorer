using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerExplorer.Web.Controllers
{
    public class RecruiterProfileController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public RecruiterProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            var currentUserId = _userManager.GetUserId(User);
            var userProfile = _unitOfWork.RecruiterProfile.GetFirstOrDefault(x => x.UserId == currentUserId);
            return View(userProfile);
        }
        [HttpPost]
        public IActionResult GetProfile(Recruiter recruiter)
        {
            _unitOfWork.RecruiterProfile.Update(recruiter);
            _unitOfWork.Save();
            return RedirectToAction("GetProfile");
        }
    }
}
