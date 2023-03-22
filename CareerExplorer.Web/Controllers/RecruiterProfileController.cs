using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerExplorer.Web.Controllers
{
    public class RecruiterProfileController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecruiterProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            var recrRep = _unitOfWork.GetRecruiterRepository();
            var currentRecrId = _userManager.GetUserId(User);
            var recrProfile = recrRep.GetFirstOrDefault(x => x.UserId == currentRecrId);
            var recrProfileDTO = _mapper.Map<RecruiterProfileDTO>(recrProfile);

            return View(recrProfileDTO);
        }
        [HttpPost]
        public IActionResult GetProfile(RecruiterProfileDTO recruiterDTO)
        {
            if(ModelState.IsValid)
            {
                var recrRep = _unitOfWork.GetRecruiterRepository();
                var recruiter = _mapper.Map<Recruiter>(recruiterDTO);
                recrRep.Update(recruiter);
                _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetProfile));
            }
            else
            {
                return View(recruiterDTO);
            }
            
        }
    }
}
