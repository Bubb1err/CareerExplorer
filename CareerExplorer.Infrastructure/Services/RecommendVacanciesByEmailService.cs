using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class RecommendVacanciesByEmailService : IRecommendVacanciesByEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IJobSeekerProfileRepository _jobSeekerProfileRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RecommendVacanciesByEmailService> _logger;
        public RecommendVacanciesByEmailService(IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            ILogger<RecommendVacanciesByEmailService> logger)
        {
            _unitOfWork= unitOfWork;
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _jobSeekerProfileRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task SendVacanciesToUsersByEmail(TimeSpan checkingPeriod)
        {
            try
            {
                var time = DateTime.Now - checkingPeriod;
                var vacancies = _vacanciesRepository.GetAll(x => x.CreatedDate >= time, "Position,Creator").ToList();
                var jobSeekers = _jobSeekerProfileRepository.GetAll(x => x.IsSubscribedToNotification, "DesiredPosition,AppUser").ToList();
                for (int i = 0; i < jobSeekers.Count(); i++)
                {
                    var vacanciesToSend = new List<Vacancy>();
                    for (int j = 0; j < vacancies.Count; j++)
                    {
                        if (vacancies[j].Position == jobSeekers[i].DesiredPosition)
                        {
                            vacanciesToSend.Add(vacancies[j]);
                        }
                    }
                    if (vacanciesToSend.Count > 0)
                    {
                        var textToSend = new StringBuilder();
                        foreach (var vacancy in vacanciesToSend)
                        {
                            textToSend
                                .Append($"<div><a href=\"https://careerexplorer.azurewebsites.net/Vacancy/GetVacancy/{vacancy.Id}\">{vacancy.Position.Name}</a><p>{vacancy.Creator.Company}</p></div>");
                        }
                        await _emailSender.SendEmailAsync(jobSeekers[i].AppUser.Email, "New vacancies", textToSend.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
