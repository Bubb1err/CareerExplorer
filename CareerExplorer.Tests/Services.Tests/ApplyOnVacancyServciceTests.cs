using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Org.BouncyCastle.Bcpg;
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
        [Fact]
        public async Task Apply_WithValidInputs_ShouldAddJobSeekerVacancyAndVacancyApplicant()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var formFileMock = new Mock<IFormFile>();
            var jobSeekerRepoMock = new Mock<IJobSeekerProfileRepository>();
            var vacanciesRepoMock = new Mock<IVacanciesRepository>();
            var jobSeekerVacancyRepoMock = new Mock<IJobSeekerVacancyRepository>();
            formFileMock.Setup(x => x.Length).Returns(277831);
            formFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());
            string userId = Guid.NewGuid().ToString();
            int jobSeekerVacancyId = 1;
            var testUser = new JobSeeker
            {
                Id = 1,
                UserId = userId,
                VacanciesApplied = new List<JobSeekerVacancy>()
            };
            jobSeekerRepoMock.Setup(r => r.GetFirstOrDefault(x => x.UserId == userId, null, true))
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
            jobSeekerVacancyRepoMock.Setup(x => x.AddAsync(It.IsAny<JobSeekerVacancy>()))
                .Callback((JobSeekerVacancy jobSeekerVacancy) => {
                    jobSeekerVacancy.VacancyId= vacancyId;
                    jobSeekerVacancy.JobSeekerId = testUser.Id;
                    jobSeekerVacancy.Id = jobSeekerVacancyId;
                    vacancy.Applicants.Add(jobSeekerVacancy);
                    testUser.VacanciesApplied.Add(jobSeekerVacancy);
                });
            jobSeekerVacancyRepoMock.Setup(r => r.GetFirstOrDefault(x => x.VacancyId == vacancyId && x.JobSeekerId == testUser.Id, null, true))
                .Returns(jobSeekerVacancy);
            vacanciesRepoMock.Setup(r => r.GetFirstOrDefault(x => x.Id == vacancyId, null, true)).Returns(vacancy);
            unitOfWorkMock.Setup(r => r.GetRepository<JobSeeker>()).Returns(jobSeekerRepoMock.Object);
            unitOfWorkMock.Setup(r => r.GetRepository<Vacancy>()).Returns(vacanciesRepoMock.Object);
            unitOfWorkMock.Setup(r => r.GetRepository<JobSeekerVacancy>()).Returns(jobSeekerVacancyRepoMock.Object);
            unitOfWorkMock.Setup(r => r.SaveAsync());
            var applyOnVacancyService = new ApplyOnVacancyService(unitOfWorkMock.Object);

            await applyOnVacancyService.Apply(testUser.UserId, vacancyId, formFileMock.Object);

            Assert.Contains(jobSeekerVacancyId, vacancy.Applicants.Select(x => x.Id));
            Assert.Contains(jobSeekerVacancyId, testUser.VacanciesApplied.Select(x => x.Id));
        }
        [Fact]
        public async Task ApplyOnVacancyWithInvalidInputs_ShouldThrowAnException()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var formFileMock = new Mock<IFormFile>();
            var jobSeekerRepoMock = new Mock<IJobSeekerProfileRepository>();
            var vacanciesRepoMock = new Mock<IVacanciesRepository>();
            var jobSeekerVacancyRepoMock = new Mock<IJobSeekerVacancyRepository>();
            formFileMock.Setup(x => x.Length).Returns(2097153);
            formFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());
            int vacancyId = 0;
            string userId = string.Empty;
            int jobSeekerId = 0;
            jobSeekerRepoMock.Setup(r => r.GetFirstOrDefault(x => x.UserId == userId, null, true))
                .Returns(null as JobSeeker);
            jobSeekerVacancyRepoMock.Setup(r => r.GetFirstOrDefault(x => x.VacancyId == vacancyId && x.JobSeekerId == jobSeekerId, null, true))
                .Returns(null as JobSeekerVacancy);
            vacanciesRepoMock.Setup(r => r.GetFirstOrDefault(x => x.Id == vacancyId, null, true)).Returns(null as Vacancy);
            unitOfWorkMock.Setup(r => r.GetRepository<JobSeeker>()).Returns(jobSeekerRepoMock.Object);
            unitOfWorkMock.Setup(r => r.GetRepository<Vacancy>()).Returns(vacanciesRepoMock.Object);
            unitOfWorkMock.Setup(r => r.GetRepository<JobSeekerVacancy>()).Returns(jobSeekerVacancyRepoMock.Object);
            unitOfWorkMock.Setup(r => r.SaveAsync());
            var applyOnVacancyService = new ApplyOnVacancyService(unitOfWorkMock.Object);

            async Task action() => await applyOnVacancyService.Apply(userId, vacancyId, formFileMock.Object);

            await Assert.ThrowsAsync<Exception>(action);
        }

    }
}
