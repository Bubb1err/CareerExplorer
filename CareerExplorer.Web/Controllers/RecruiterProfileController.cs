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
        private readonly IRecruiterProfileRepository _recruiterRepository;
        public RecruiterProfileController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recruiterRepository = _unitOfWork.GetRecruiterRepository();
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            var currentRecrId = _userManager.GetUserId(User);
            var recrProfile = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecrId);
            var recrProfileDTO = _mapper.Map<RecruiterProfileDTO>(recrProfile);

            return View(recrProfileDTO);
        }
        [HttpPost]
        public IActionResult GetProfile(RecruiterProfileDTO recruiterDTO)
        {
            if(ModelState.IsValid)
            {
                var recruiter = _mapper.Map<Recruiter>(recruiterDTO);
                _recruiterRepository.Update(recruiter);
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
