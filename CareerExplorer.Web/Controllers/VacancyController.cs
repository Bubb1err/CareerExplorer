using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Web.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerExplorer.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var vacRep = _unitOfWork.GetVacanciesRepository();
            var vacancies = vacRep.GetAll(x => x.IsAvailable == true);
            var vacanciesDto = _mapper.Map<List<VacancyDTO>>(vacancies);
            return View(vacanciesDto);
        }
        [HttpGet]
        public ActionResult CreatedVacancies()
        {
            var vacRep = _unitOfWork.GetVacanciesRepository();
            var currentRecrId = _userManager.GetUserId(User);
            var recRep = _unitOfWork.GetRecruiterRepository();
            var creatorId = recRep.GetFirstOrDefault(x => x.UserId== currentRecrId).Id;
            var vacancies = vacRep.GetAll(x => x.CreatorId == creatorId).ToList();
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
                var recRep = _unitOfWork.GetRecruiterRepository();
                var creatorId = recRep.GetFirstOrDefault(x => x.UserId == currentRecrId).Id;
                var vacancy = new Vacancy()
                {
                    Title= vacancyDTO.Title,
                    Description= vacancyDTO.Description,
                    IsAvailable = vacancyDTO.IsAvailable,
                    CreatorId = creatorId,
                    CreatedDate= vacancyDTO.CreatedDate
                };
                var vacRep = _unitOfWork.GetVacanciesRepository();
                vacRep.Add(vacancy);
                _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(CreatedVacancies));
        }
        [HttpGet]
        public ActionResult Edit(int? id) 
        { 
            if (id == null)
                return BadRequest();
            var vacRep = _unitOfWork.GetVacanciesRepository();
            var vacancy = vacRep.GetFirstOrDefault(x => x.Id == id);
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
            return View(vacancyDto); 
        }
        [HttpPost]
        public ActionResult Edit(VacancyDTO vacancyDto)
        {
            if(vacancyDto == null)
                return BadRequest();
            var vacancy = _mapper.Map<Vacancy>(vacancyDto);
            var vacRep = _unitOfWork.GetVacanciesRepository();
            vacRep.Update(vacancy);
            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(CreatedVacancies));
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var vacRep = _unitOfWork.GetVacanciesRepository();
            var vacancy = vacRep.GetFirstOrDefault(x => x.Id == id);
            var vacancyDto = _mapper.Map<VacancyDTO>(vacancy);
            return View(vacancyDto);
        }
        [HttpPost]
        public ActionResult Delete(VacancyDTO vacancyDto)
        {
            if (vacancyDto == null)
                return BadRequest();
            var vacancy = _mapper.Map<Vacancy>(vacancyDto);
            var vacRep = _unitOfWork.GetVacanciesRepository();
            vacRep.Remove(vacancy);
            _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(CreatedVacancies));
        }

    }
}
