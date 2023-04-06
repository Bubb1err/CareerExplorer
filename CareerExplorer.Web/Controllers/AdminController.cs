using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CareerExplorer.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdminRepository _adminRepository;
        private readonly IRepository<SkillsTag> _skillsRepository;
        private readonly IRepository<Position> _positionRepository;
        private readonly IMapper _mapper;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminService _adminService;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IAdminService adminService)
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
            _userManager = userManager;
            _skillsRepository = _unitOfWork.GetRepository<SkillsTag>();
            _positionRepository = _unitOfWork.GetRepository<Position>();
            _adminRepository = _unitOfWork.GetAdminRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _recruiterRepository = _unitOfWork.GetRecruiterRepository();
            _appUserRepository = _unitOfWork.GetRepository<AppUser>();
            _adminService = adminService;
        }
        #region SkillTag Control
        [HttpGet]
        public IActionResult GetSkillTags(string search)
        {
            var tags = _skillsRepository.GetAll(t => t.Title.StartsWith(search)).ToList();
            return Ok(tags);
        }
        public IActionResult GetSkillsTags()
        {
            var skillsTags = _skillsRepository.GetAll();
            var skillsTagsDto = _mapper.Map<List<SkillTagDTO>>(skillsTags);
            return View(skillsTagsDto);
        }
        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTag(SkillTagDTO skillTagDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(skillTagDTO);
                }
                if (skillTagDTO == null)
                    return BadRequest(ModelState);

                var tag = _mapper.Map<SkillsTag>(skillTagDTO);
                await _skillsRepository.AddAsync(tag);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetSkillsTags));
            }
            catch { return BadRequest(); }

        }
        [HttpGet]
        public async Task<IActionResult> EditTag(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var tag = _skillsRepository.GetFirstOrDefault(x => x.Id == id);
                var skillsTagDto = _mapper.Map<SkillTagDTO>(tag);
                return View(skillsTagDto);
            }
            catch { return BadRequest(); }

        }
        [HttpPost]
        public async Task<IActionResult> EditTag(SkillTagDTO skillTagDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var skillTag = _mapper.Map<SkillsTag>(skillTagDto);
                    _adminRepository.UpdateSkillTag(skillTag);
                    await _unitOfWork.SaveAsync();
                    return RedirectToAction(nameof(GetSkillsTags));
                }
                return View(skillTagDto);
            }
            catch { return BadRequest(); }

        }
        [HttpGet]
        public async Task<IActionResult> DeleteTag(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var skillTag = _skillsRepository.GetFirstOrDefault(x => x.Id==id);
                if (skillTag == null)
                    return BadRequest();

                _skillsRepository.Remove(skillTag);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetSkillsTags));
            }
            catch { return BadRequest(); }
        }
        #endregion
        #region PositionControl
        public IActionResult GetPositions()
        {
            var positions = _positionRepository.GetAll();
            var positionsDto = _mapper.Map<List<PositionDTO>>(positions);
            return View(positionsDto);
        }
        [HttpGet]
        public IActionResult CreatePosition()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePosition(PositionDTO positionDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return View(positionDto);
                if (positionDto == null)
                    return BadRequest(ModelState);

                var position = _mapper.Map<Position>(positionDto);
                await _positionRepository.AddAsync(position);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetPositions));
            }
            catch { return BadRequest(); }

        }
        [HttpGet]
        public async Task<IActionResult> EditPosition(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var position = _positionRepository.GetFirstOrDefault(x => x.Id == id);
                var postionDto = _mapper.Map<PositionDTO>(position);
                return View(postionDto);
            }
            catch { return BadRequest(); }

        }
        [HttpPost]
        public async Task<IActionResult> EditPosition(PositionDTO positionDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var postion = _mapper.Map<Position>(positionDto);
                    _adminRepository.UpdatePosition(postion);
                    await _unitOfWork.SaveAsync();
                    return RedirectToAction(nameof(GetPositions));
                }
                return View(positionDto);
            }
            catch { return BadRequest(); }

        }
        [HttpGet]
        public async Task<IActionResult> DeletePosition(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();
                var position = _positionRepository.GetFirstOrDefault(x => x.Id==id);
                if (position == null)
                    return BadRequest();

                _positionRepository.Remove(position);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(GetPositions));
            }
            catch { return BadRequest(); }
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> GetVacanciesToAccept()
        {
            var vacancies = _vacanciesRepository.GetAll(x => x.IsAccepted == false, "Position");
            var vacanciesDto = _mapper.Map<List<VacancyDTO>>(vacancies);
            foreach (var vacancyDto in vacanciesDto)
            {
                var creator = _recruiterRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CreatorId);
                if (creator == null)
                    return BadRequest();
                var appUserCreator = _appUserRepository.GetFirstOrDefault(x => x.RecruiterProfileId == creator.Id);
                if (appUserCreator == null)
                    return BadRequest();
                var creatorEmail = await _userManager.GetEmailAsync(appUserCreator);
                if (creatorEmail == null)
                    return BadRequest();
                vacancyDto.CreatorNickName = creatorEmail;
            }
            return View(vacanciesDto);
        }
        [HttpGet]
        public async Task<IActionResult> ViewVacancy(int id)
        {
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id, "Position");
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
            var creator = _recruiterRepository.GetFirstOrDefault(x => x.Id == vacancy.CreatorId);
            var appUserCreator = _appUserRepository.GetFirstOrDefault(x => x.RecruiterProfileId == creator.Id);
            var creatorEmail = await _userManager.GetEmailAsync(appUserCreator);
            vacancyDto.CreatorName = creator.Name;
            vacancyDto.CreatorSurname = creator.Surname;
            if (creatorEmail == null)
                return BadRequest();
            vacancyDto.CreatorNickName = creatorEmail;
            return View(vacancyDto);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptVacancy(int id)
        {
            try
            {
                if(id==0) return BadRequest();
                await _adminService.AcceptVacancy(id);
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }
    }
}
