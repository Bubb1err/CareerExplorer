using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CareerExplorer.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly IJobSeekerProfileRepository _jobSeekerRepositoy;
        private readonly IRepository<JobSeekerVacancy> _jobSeekerVacancyRepository;
        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _recruiterRepository = _unitOfWork.GetRecruiterRepository();
            _jobSeekerRepositoy = _unitOfWork.GetJobSeekerRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetRepository<JobSeekerVacancy>();
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var vacancies = _vacanciesRepository.GetAvailableVacancies().ToList();
            var vacanciesDto = _mapper.Map<List<VacancyDTO>>(vacancies);
            return View(vacanciesDto);
        }
        [HttpGet]
        public ActionResult CreatedVacancies()
        {
            var currentRecrId = _userManager.GetUserId(User);

            var vacancies = _vacanciesRepository.GetCreatedVacancies(currentRecrId).ToList();
            var vacanciesDTO = _mapper.Map<List<VacancyDTO>>(vacancies);
            return View(vacanciesDTO);

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(VacancyDTO vacancyDTO)
        {
            if(vacancyDTO == null)
                return BadRequest(ModelState);
            var currentRecrId = _userManager.GetUserId(User);

            var creatorId = _recruiterRepository.GetFirstOrDefault(x => x.UserId == currentRecrId).Id;
            var vacancy = _mapper.Map<Vacancy>(vacancyDTO);
            vacancy.CreatorId = creatorId;
            _vacanciesRepository.Add(vacancy);

            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(CreatedVacancies));
        }
        [HttpGet]
        public ActionResult Edit(int? id) 
        { 
            if (id == null)
                return BadRequest();
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
            return View(vacancyDto); 
        }
        [HttpPost]
        public ActionResult Edit(VacancyDTO vacancyDto)
        {
            if(vacancyDto == null)
                return BadRequest();
            var vacancy = _mapper.Map<Vacancy>(vacancyDto);
            _vacanciesRepository.Update(vacancy);
            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(CreatedVacancies));
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
            return View(vacancyDto);
        }
        [HttpPost]
        public ActionResult Delete(VacancyDTO vacancyDto)
        {
            if (vacancyDto == null)
                return BadRequest();
            var vacancy = _mapper.Map<Vacancy>(vacancyDto);
            _vacanciesRepository.Remove(vacancy);
            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(CreatedVacancies));
        }
        [HttpGet]
        public ActionResult GetVacancy(int? id)
        {
            if(id == null) 
                return BadRequest();
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);

            var currentUserId = _userManager.GetUserId(User);

            var joobSeekId = _jobSeekerRepositoy.GetFirstOrDefault(x => x.UserId == currentUserId).Id;

            //if jobSeekerVacancy object exist then jobSeeker has already applied on vacancy
            var applied = _jobSeekerVacancyRepository.GetFirstOrDefault(x=> x.VacancyId== id && x.JobSeekerId == joobSeekId);
            if (applied != null)
                vacancyDto.IsApplied= true;
            else
                vacancyDto.IsApplied= false;
            return View(vacancyDto);
        }

    }
}
