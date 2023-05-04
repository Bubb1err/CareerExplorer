using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Tests.Services.Tests
{
    public class ApplyOnVacancyServciceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFormFile> _formFileMock;
        private readonly Mock<IJobSeekerProfileRepository> _jobSeekerProfileRepositoryMock;
        private readonly Mock<IVacanciesRepository> _vacanciesRepositoryMock;
        private readonly Mock<IJobSeekerVacancyRepository> _jobSeekerVacancyRepositoryMock;
        private readonly ApplyOnVacancyService _applyOnVacancyService;
        public ApplyOnVacancyServciceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _formFileMock= new Mock<IFormFile>();
            _jobSeekerProfileRepositoryMock = new Mock<IJobSeekerProfileRepository>();
            _vacanciesRepositoryMock= new Mock<IVacanciesRepository>();
            _jobSeekerVacancyRepositoryMock = new Mock<IJobSeekerVacancyRepository>();

            _unitOfWorkMock.Setup(x => x.GetRepository<Vacancy>()).Returns(_vacanciesRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.GetRepository<JobSeeker>()).Returns(_jobSeekerProfileRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.GetRepository<JobSeekerVacancy>()).Returns(_jobSeekerVacancyRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.SaveAsync());
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());
            _applyOnVacancyService = new ApplyOnVacancyService(_unitOfWorkMock.Object);
        }
        [Fact]
        public async Task Apply_WithValidInputs_ShouldAddJobSeekerVacancyAndVacancyApplicant()
        {
            _formFileMock.Setup(x => x.Length).Returns(277831);            
            string userId = Guid.NewGuid().ToString();
            int jobSeekerVacancyId = 1;
            var testUser = new JobSeeker
            {
                Id = 1,
                UserId = userId,
                VacanciesApplied = new List<JobSeekerVacancy>()
            };
            _jobSeekerProfileRepositoryMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<JobSeeker, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>()))
                .Returns(testUser);
            var vacancyId = 1;
            var vacancy = new Vacancy
            {
                Id = vacancyId,
                IsAvailable = true,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
                " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
                "ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse " +
                "cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, " +
                "sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Applicants = new List<JobSeekerVacancy>(),
                CreatorId = 1
            };
            var jobSeekerVacancy = new JobSeekerVacancy
            {
                VacancyId = vacancyId,
                JobSeekerId = testUser.Id,
                Id = jobSeekerVacancyId
            };
            _jobSeekerVacancyRepositoryMock.Setup(x => x.AddAsync(It.IsAny<JobSeekerVacancy>()))
                .Callback((JobSeekerVacancy jobSeekerVacancy) => {
                    jobSeekerVacancy.VacancyId= vacancyId;
                    jobSeekerVacancy.JobSeekerId = testUser.Id;
                    jobSeekerVacancy.Id = jobSeekerVacancyId;
                    vacancy.Applicants.Add(jobSeekerVacancy);
                    testUser.VacanciesApplied.Add(jobSeekerVacancy);
                });
            _jobSeekerVacancyRepositoryMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<JobSeekerVacancy, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>()))
                .Returns(jobSeekerVacancy);
            _vacanciesRepositoryMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Vacancy, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>())).Returns(vacancy);;

            await _applyOnVacancyService.Apply(testUser.UserId, vacancyId, _formFileMock.Object);

            Assert.Contains(jobSeekerVacancyId, vacancy.Applicants.Select(x => x.Id));
            Assert.Contains(jobSeekerVacancyId, testUser.VacanciesApplied.Select(x => x.Id));
        }
        [Fact]
        public async Task ApplyOnVacancyWithInvalidInputs_ShouldThrowAnException()
        {
            _formFileMock.Setup(x => x.Length).Returns(2097153);           
            int vacancyId = 0;
            string userId = string.Empty;
            int jobSeekerId = 0;
            _jobSeekerProfileRepositoryMock
                .Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<JobSeeker, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>()))
                .Returns(null as JobSeeker);
            _jobSeekerVacancyRepositoryMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<JobSeekerVacancy, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>()))
                .Returns(null as JobSeekerVacancy);
            _vacanciesRepositoryMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Vacancy, bool>>>(),
                It.IsAny<string?>(), It.IsAny<bool>())).Returns(null as Vacancy);

            async Task action() => await _applyOnVacancyService.Apply(userId, vacancyId, _formFileMock.Object);

            await Assert.ThrowsAsync<Exception>(action);
        }

    }
}
