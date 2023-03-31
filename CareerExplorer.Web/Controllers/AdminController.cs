using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Shared;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Authorization;
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
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
            _skillsRepository = _unitOfWork.GetRepository<SkillsTag>();
            _positionRepository = _unitOfWork.GetRepository<Position>();
            _adminRepository = _unitOfWork.GetAdminRepository();
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
    }
}
