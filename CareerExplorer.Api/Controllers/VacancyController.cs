using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CareerExplorer.Api.DTO;
using Microsoft.AspNetCore.Authorization;


namespace CareerExplorer.Api.Controllers
{
    [ApiController]
    public class VacancyController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IMapper _mapper;
        public VacancyController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _mapper= mapper;
            _response = new();
        }

        [HttpGet]
        [Route("api/vacancies")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAll([FromQuery]int pageSize = 5, int pageNumber = 1)
        {
            try
            {
                var vacancies = _vacanciesRepository
                    .GetAvailablePaginatedAndFilteredVacancies(pageSize, pageNumber, out int count);
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
            }
            return _response;
        }
    }
}
