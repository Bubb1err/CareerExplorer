using AutoMapper;
using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Shared;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerExplorer.Api.Controllers
{
    [ApiController]
    public class SkillTagsController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepository<SkillsTag> _skillTagRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdminRepository _adminRepository;
        public SkillTagsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _response = new();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _skillTagRepository = _unitOfWork.GetRepository<SkillsTag>();
            _adminRepository = (IAdminRepository)_unitOfWork.GetRepository<Admin>();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("api/skilltags")]
        public ActionResult<APIResponse> GetAll(string? search, [FromQuery]int pageSize = 5,[FromQuery] int pageNumber = 1)
        {
            try
            {
                IEnumerable<SkillsTag> skillTags;
                if(!string.IsNullOrWhiteSpace(search))
                {
                    skillTags = _skillTagRepository.GetAll(x => x.Title.ToLower().Contains(search.ToLower()));
                }
                else
                {
                    skillTags = _skillTagRepository.GetAll();
                }

                if(skillTags == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var paginatedSkillTags = _skillTagRepository
                    .Paginate(skillTags.AsQueryable(), pageSize, pageNumber).ToList();
                var skillTagsDto = _mapper.Map<List<SkillTagDTO>>(paginatedSkillTags);
                _response.Result = skillTagsDto;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);

            } catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("api/skilltags/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<APIResponse> GetSkillTag(int id)
        {
            try
            {
                var skillTag = _skillTagRepository.GetFirstOrDefault(x => x.Id == id);
                if(skillTag == null)
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { $"Skilltag with id {id} was not found." };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var skillTagDto = _mapper.Map<SkillTagDTO>(skillTag);
                _response.Result = skillTagDto;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("api/skilltag")]
        public async Task<ActionResult<APIResponse>> Create([FromQuery]string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Title could not be empty." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                SkillsTag skillTag = new SkillsTag
                {
                    Title = title
                };
                await _skillTagRepository.AddAsync(skillTag);
                await _unitOfWork.SaveAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = "Skilltag has been created successfully.";
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
        [HttpPatch]
        [Route("api/skilltag")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Edit([FromForm]SkillTagDTO skillTagDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(skillTagDto.Title))
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Title could not be empty." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if(skillTagDto.Id == 0)
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Incorrect id." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var skillTag = _mapper.Map<SkillsTag>(skillTagDto);
                _adminRepository.UpdateSkillTag(skillTag);
                await _unitOfWork.SaveAsync();
                _response.IsSuccess = true;
                _response.Result = "Skilltag has been edited successfully.";
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpDelete]
        [Route("api/skilltag/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Incorrect id." };
                    return BadRequest(_response);
                }
                var skillTag = _skillTagRepository.GetFirstOrDefault(x => x.Id == id);
                if(skillTag == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { $"Skill tag with id {id} was not found." };
                    return NotFound(_response);
                }
                _skillTagRepository.Remove(skillTag);
                await _unitOfWork.SaveAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = $"Skill tag with id {id} has been deleted successfully.";
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
    }
}
