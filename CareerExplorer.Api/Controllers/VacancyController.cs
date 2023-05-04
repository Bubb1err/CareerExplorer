using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Web;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CareerExplorer.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Shared;
using CareerExplorer.Infrastructure.Repository;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;

namespace CareerExplorer.Api.Controllers
{
    [EnableCors("LocalPolicy, Other")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVacancyService _vacancyService;
        private readonly ICountryRepository _countryRepository;
        private readonly IRepository<City> _cityRepository;
        public VacancyController(IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IVacancyService vacancyService)
        {
            _unitOfWork = unitOfWork;
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _mapper= mapper;
            _response = new();
            _userManager = userManager;
            _vacancyService = vacancyService;
            _cityRepository = _unitOfWork.GetRepository<City>();
            _countryRepository = (ICountryRepository)_unitOfWork.GetRepository<Country>();
        }

        [HttpGet]
        [Route("api/vacancies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<APIResponse> GetAll([FromQuery]int pageSize = 5, [FromQuery] int pageNumber = 1,
            [FromQuery] int[]? tagIds = null, [FromQuery]int[]? types = null)
        {
            try
            {
                List<Vacancy> vacancies;
                Expression<Func<Vacancy, bool>> filter = null;
                if (tagIds == null && types != null)
                {
                    filter = x => x.WorkType != null && types.Contains((int)x.WorkType);
                }
                else if (tagIds != null && types == null)
                {
                    filter = x => x.Requirements.Any(x => types.Contains(x.Id));
                }
                else if (tagIds != null && types != null)
                {
                    filter = x => x.WorkType != null && types.Contains((int)x.WorkType)
                            && x.Requirements.Any(x => types.Contains(x.Id));
                }
                vacancies = _vacanciesRepository.GetAvailablePaginatedAndFilteredVacancies(StaticData.GetAllVacanciesPageSize,
                    pageNumber, out int totalVacancies, filter).ToList();
                if(vacancies == null)
                {
                    _response.IsSuccess= false;
                    _response.StatusCode= HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var vacanciesDTO = _mapper.Map<List<VacancyDTO>>(vacancies);
                _response.Result = vacanciesDTO;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess= true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess= false;
                _response.StatusCode= HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpGet]
        [Route("api/vacancies/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<APIResponse> GetVacancy(int id)
        {
            try
            {
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id, "Creator,Country,City,Position,Requirements");
                if(vacancy == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { $"Vacancy with id {id} was not found." };
                    return NotFound(_response);
                }
                var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = vacancyDto;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Recruiter)]
        [Route("api/vacancy")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateVacancy([FromBody]CreateVacancyDTO vacancyDto, 
            [FromQuery]int position, [FromQuery]string[] requiredSkills)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    if(vacancyDto.Description.Length < 200)
                    {
                        ModelState.AddModelError("Vacancy Error", "Description must contain more then 200 symbols.");
                    }
                    if(vacancyDto.Salary < 0 || vacancyDto.Salary > 20000)
                    {
                        ModelState.AddModelError("Vacancy Error", "Salary must be between 0 and 20000");
                    }
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                string currentRecruiterId = _userManager.GetUserId(User);
                if(currentRecruiterId == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                var vacancy = _mapper.Map<Vacancy>(vacancyDto);
                await _vacancyService.CreateVacancy(requiredSkills, position, currentRecruiterId, vacancy);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = "Vacancy was created successfully";
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
        [Authorize(Roles = UserRoles.Recruiter)]
        [Route("api/vacancies/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVacancy(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { "Incorrect id." };
                    return BadRequest(_response);
                }
                var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
                if(vacancy == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Errors = new List<string> { $"Vacancy with id {id} was not found. Cannot delete." };
                    return NotFound(_response);
                }
                _vacanciesRepository.Remove(vacancy);
                await _unitOfWork.SaveAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Vacancy has been deleted successfully.";
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.Result = new List<string> { ex.Message };
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
        [HttpPatch]
        [Authorize(Roles = UserRoles.Recruiter)]
        [Route("api/vacancies")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> EditVacancy([FromForm]EditVacancyDTO vacancyDto,
            [FromQuery] string[] tags, [FromQuery] int position)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var currentVacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == vacancyDto.Id, "Requirements");
                if(currentVacancy == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { $"Vacancy with id {vacancyDto.Id} was not found." };
                    return NotFound(_response);
                }
                currentVacancy.Description = vacancyDto.Description;
                currentVacancy.IsAvailable = vacancyDto.IsAvailable;
                var country = _countryRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CountryId);
                var city = _cityRepository.GetFirstOrDefault(x => x.Id == vacancyDto.CityId);
                currentVacancy.Country = country;
                currentVacancy.City = city;
                currentVacancy.ExperienceYears = vacancyDto.ExperienceYears;
                currentVacancy.Salary = vacancyDto.Salary;
                currentVacancy.EnglishLevel = vacancyDto.EnglishLevel;
                currentVacancy.WorkType = vacancyDto.WorkType;
                await _vacancyService.EditVacancy(tags, currentVacancy, position);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Vacancy has been edited successfully.";
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
    }
}
