using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Tests.Services.Tests
{
    public class AdminServiceTests
    {
        [Fact]
        public async Task AcceptVacancy_ShouldSetPropertyToTrue()
        {
            var vacanciesRepo = new Mock<IVacanciesRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.GetRepository<Vacancy>()).Returns(vacanciesRepo.Object);
            unitOfWork.Setup(x => x.SaveAsync());
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
            vacanciesRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == vacancyId, null, true)).Returns(vacancy);
            var adminService = new AdminService(unitOfWork.Object);

            await adminService.AcceptVacancy(vacancyId);

            Assert.True(vacancy.IsAccepted);
        }
        [Fact]
        public async Task AcceptVacancyWithInvalidId_ShouldThrowAnException()
        {
            var vacanciesRepo = new Mock<IVacanciesRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.GetRepository<Vacancy>()).Returns(vacanciesRepo.Object);
            int vacancyId = 0;
            vacanciesRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == vacancyId, null, true)).Returns(null as Vacancy);
            var adminService = new AdminService(unitOfWork.Object);

            async Task action() => await adminService.AcceptVacancy(vacancyId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public async Task AcceptJobSeeker_ShouldSetPropToTrue()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var jobSeekerProfileRepo = new Mock<IJobSeekerProfileRepository>();
            unitOfWork.Setup(x => x.GetRepository<JobSeeker>()).Returns(jobSeekerProfileRepo.Object);
            unitOfWork.Setup(x => x.SaveAsync());
            int jobSeekerId = 1;
            var jobseeker = new JobSeeker
            {
                Id = jobSeekerId,
                IsAccepted = false
            };
            jobSeekerProfileRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == jobSeekerId, null, true)).Returns(jobseeker);
            var adminService = new AdminService(unitOfWork.Object);

            await adminService.AcceptJobSeekerProfile(jobSeekerId);

            Assert.True(jobseeker.IsAccepted);
        }
        [Fact]
        public async Task AcceptJobSeekerProfileWithInvalidId_ShouldThrowAnException()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var jobSeekerProfileRepo = new Mock<IJobSeekerProfileRepository>();
            unitOfWork.Setup(x => x.GetRepository<JobSeeker>()).Returns(jobSeekerProfileRepo.Object);
            int jobSeekerId = 0;
            jobSeekerProfileRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == jobSeekerId, null, true)).Returns(null as JobSeeker);
            var adminService = new AdminService(unitOfWork.Object);

            async Task action() => await adminService.AcceptJobSeekerProfile(jobSeekerId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public async Task AcceptRecruiterProfile_ShouldSetPropToTrue()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var recruiterProfileRepo = new Mock<IRecruiterProfileRepository>();
            unitOfWork.Setup(x => x.GetRepository<Recruiter>()).Returns(recruiterProfileRepo.Object);
            unitOfWork.Setup(x => x.SaveAsync());
            int recruiterId = 1;
            var recruiter = new Recruiter
            {
                Id = recruiterId,
                IsAccepted=true
            };
            recruiterProfileRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == recruiterId, null, true)).Returns(recruiter);
            var adminService = new AdminService(unitOfWork.Object);

            await adminService.AcceptRecruiterProfile(recruiterId);

            Assert.True(recruiter.IsAccepted);
        }
        [Fact]
        public async Task AcceptRecruiterProfileWithInvalidId_ShouldThrowAnException()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var recruiterProfileRepo = new Mock<IRecruiterProfileRepository>();
            unitOfWork.Setup(x => x.GetRepository<Recruiter>()).Returns(recruiterProfileRepo.Object);
            int recruiterId = 0;
            recruiterProfileRepo.Setup(x => x.GetFirstOrDefault(x => x.Id == recruiterId, null, true)).Returns(null as Recruiter);
            var adminService = new AdminService(unitOfWork.Object);

            async Task action() => await adminService.AcceptRecruiterProfile(recruiterId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public void IsJobSeekerProfileFilled_ShouldReturnTrue()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var jobSeeker = new JobSeeker
            {
                Id = 1,
                Name = "Test",
                Surname = "Test",
                Experience = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et" +
                " dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo" +
                " consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur." +
                " Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Phone = "72801927",
                Skills = new List<SkillsTag>
                {
                    new SkillsTag()
                },
                EnglishLevel= 1,
                ExperienceYears = 1,
                DesiredPosition = new Position(),
                Country= new Country(),
                City= new City()
            };
            var adminService = new AdminService(unitOfWork.Object);

            bool result = adminService.IsJobSeekerProfileFilled(jobSeeker);

            Assert.True(result);
        }
        [Fact]
        public void IsJobSeekerProfileFilled_ShouldReturnFalse()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var jobSeeker = new JobSeeker
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                Experience = "too short string",
                Phone = string.Empty
            };
            var adminService = new AdminService(unitOfWork.Object);

            bool result = adminService.IsJobSeekerProfileFilled(jobSeeker);

            Assert.False(result);
        }
        [Fact]
        public void IsRecruiterProfileFilled_ShoulReturnTrue()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var recruiter = new Recruiter
            {
                Id = 1,
                Name = "Test",
                Surname = "Test",
                Company = "Test",
                CompanyDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et" +
                " dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo " +
                "consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur" +
                " sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };
            var adminService = new AdminService(unitOfWork.Object);

            bool result = adminService.IsRecuiterProfileFilled(recruiter);

            Assert.True(result);
        }
        [Fact]
        public void IsRecruiterProfileFilled_ShouldRetrunFalse()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var recruiter = new Recruiter
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                Company = string.Empty,
                CompanyDescription = "too short string"
            };
            var adminService = new AdminService(unitOfWork.Object);

            bool result = adminService.IsRecuiterProfileFilled(recruiter);

            Assert.False(result);
        }
    }
}
