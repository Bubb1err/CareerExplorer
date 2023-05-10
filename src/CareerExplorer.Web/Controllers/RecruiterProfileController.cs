using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Infrastructure.Services;
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
        private readonly ILogger<RecruiterProfileController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly IAdminService _adminService;
        public RecruiterProfileController(UserManager<IdentityUser> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper, IAdminService adminService,
            ILogger<RecruiterProfileController> logger)
        {
            _adminService = adminService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recruiterRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _logger = logger;
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
                _logger.Log(LogLevel.Error, ex.ToString());
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
                    recruiter.IsFilled = _adminService.IsRecuiterProfileFilled(recruiter);
                    recruiter.IsAccepted = false;
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
