using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerExplorer.Web.Controllers
{
    [Authorize(Roles = UserRoles.Recruiter)]
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
            try
            {
                var currentRecruiterId = _userManager.GetUserId(User);
                var recruiterProfile = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecruiterId);
                if (recruiterProfile == null)
                {
                    return NotFound();
                }
                var recruiterProfileDTO = _mapper.Map<RecruiterProfileDTO>(recruiterProfile);

                return View(recruiterProfileDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest();
            
        }
        [HttpPost]
        public async Task<IActionResult> GetProfile(RecruiterProfileDTO recruiterDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var recruiter = _mapper.Map<Recruiter>(recruiterDTO);
                    _recruiterRepository.Update(recruiter);
                    await _unitOfWork.SaveAsync();
                    return RedirectToAction(nameof(GetProfile));
                }
                else
                {
                    return View(recruiterDTO);
                }
            }
            catch { return BadRequest(); }
            
            
        }
    }
}
