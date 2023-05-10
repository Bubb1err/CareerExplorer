using AutoMapper;
using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Shared;
using CareerExplorer.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerExplorer.Api.Controllers
{
    [EnableCors("LocalPolicy, Other")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected APIResponse _response;
        private readonly IRepository<Position> _positionsRepository;
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;
        public PositionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
            _response= new APIResponse();
            _positionsRepository = _unitOfWork.GetRepository<Position>();
            _adminRepository = (IAdminRepository)_unitOfWork.GetRepository<Admin>();
        }
        [HttpGet]
        [Route("api/positions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<APIResponse> GetAll(string? search, 
            [FromQuery] int pageSize = 5, [FromQuery] int pageNumber = 1)
        {
            try
            {
                IEnumerable<Position> positions;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    positions = _positionsRepository.GetAll(x => x.Name.ToLower().Contains(search.ToLower()));
                }
                else
                {
                    positions = _positionsRepository.GetAll();
                }

                if (positions == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var paginatedPositions = _positionsRepository
                    .Paginate(positions.AsQueryable(), pageSize, pageNumber).ToList();
                var positionsDto = _mapper.Map<List<PositionDTO>>(paginatedPositions);
                _response.Result = positionsDto;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("api/positions/{id:int}")]
        public ActionResult<APIResponse> Get(int id)
        {
            try
            {
                var position = _positionsRepository.GetFirstOrDefault(x => x.Id == id);
                if (position == null)
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { $"Position with id {id} was not found." };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var positionDto = _mapper.Map<PositionDTO>(position);
                _response.Result = positionDto;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Route("api/position")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Create([FromQuery]string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Name could not be empty." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Position position = new Position
                {
                    Name= name
                };
                await _positionsRepository.AddAsync(position);
                await _unitOfWork.SaveAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = "Position has been created successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
        [HttpPatch]
        [Route("api/position")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Edit([FromForm]PositionDTO positionDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionDto.Name))
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Name could not be empty." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (positionDto.Id == 0)
                {
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Incorrect id." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var position = _mapper.Map<Position>(positionDto);
                _adminRepository.UpdatePosition(position);
                await _unitOfWork.SaveAsync();
                _response.IsSuccess = true;
                _response.Result = "Position has been edited successfully.";
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpDelete]
        [Route("api/position/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Incorrect id." };
                    return BadRequest(_response);
                }
                var position = _positionsRepository.GetFirstOrDefault(x => x.Id == id);
                if (position == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { $"Position with id {id} was not found." };
                    return NotFound(_response);
                }
                _positionsRepository.Remove(position);
                await _unitOfWork.SaveAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = $"Position with id {id} has been deleted successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
    }
}
