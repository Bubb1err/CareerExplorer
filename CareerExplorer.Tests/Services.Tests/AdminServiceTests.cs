using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Services;
using Moq;
using System.Linq.Expressions;

namespace CareerExplorer.Tests.Services.Tests
{
    public class AdminServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVacanciesRepository> _vacanciesRepositoryMock;
        private readonly Mock<IJobSeekerProfileRepository> _jobSeekerProfileRepositoryMock;
        private readonly Mock<IRecruiterProfileRepository> _recruiterProfileRepositoryMock;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _vacanciesRepositoryMock = new Mock<IVacanciesRepository>();
            _jobSeekerProfileRepositoryMock = new Mock<IJobSeekerProfileRepository>();
            _recruiterProfileRepositoryMock = new Mock<IRecruiterProfileRepository>();

            _unitOfWorkMock.Setup(x => x.GetRepository<Vacancy>()).Returns(_vacanciesRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.GetRepository<JobSeeker>()).Returns(_jobSeekerProfileRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.GetRepository<Recruiter>()).Returns(_recruiterProfileRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.SaveAsync());
            _adminService = new AdminService(_unitOfWorkMock.Object);
        }
        [Fact]
        public async Task AcceptVacancy_ShouldSetPropertyToTrue()
        {
            var vacancyId = 1;
            var vacancy = new Vacancy
            {
                Id = vacancyId,
                IsAccepted= false,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
                " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
                "ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse " +
                "cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, " +
                "sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Applicants = new List<JobSeekerVacancy>(),
                CreatorId = 1
            };
            _vacanciesRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<Vacancy, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(vacancy);

            await _adminService.AcceptVacancy(vacancyId);

            Assert.True(vacancy.IsAccepted);
        }
        [Fact]
        public async Task AcceptVacancyWithInvalidId_ShouldThrowAnException()
        {
            int vacancyId = 0;
            _vacanciesRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<Vacancy, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(null as Vacancy);

            async Task action() => await _adminService.AcceptVacancy(vacancyId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public async Task AcceptJobSeeker_ShouldSetPropToTrue()
        {
            int jobSeekerId = 1;
            var jobseeker = new JobSeeker
            {
                Id = jobSeekerId,
                IsAccepted = false
            };
            _jobSeekerProfileRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<JobSeeker, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(jobseeker);

            await _adminService.AcceptJobSeekerProfile(jobSeekerId);

            Assert.True(jobseeker.IsAccepted);
        }
        [Fact]
        public async Task AcceptJobSeekerProfileWithInvalidId_ShouldThrowAnException()
        {
            int jobSeekerId = 0;
            _jobSeekerProfileRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<JobSeeker, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(null as JobSeeker);

            async Task action() => await _adminService.AcceptJobSeekerProfile(jobSeekerId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public async Task AcceptRecruiterProfile_ShouldSetPropToTrue()
        {
            int recruiterId = 1;
            var recruiter = new Recruiter
            {
                Id = recruiterId,
                IsAccepted=true
            };
            _recruiterProfileRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<Recruiter, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(recruiter);

            await _adminService.AcceptRecruiterProfile(recruiterId);

            Assert.True(recruiter.IsAccepted);
        }
        [Fact]
        public async Task AcceptRecruiterProfileWithInvalidId_ShouldThrowAnException()
        {
            int recruiterId = 0;
            _recruiterProfileRepositoryMock.Setup(x => x.GetFirstOrDefault
            (It.IsAny<Expression<Func<Recruiter, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(null as Recruiter);

            async Task action() => await _adminService.AcceptRecruiterProfile(recruiterId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
        [Fact]
        public void IsJobSeekerProfileFilled_ShouldReturnTrue()
        {
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

            bool result = _adminService.IsJobSeekerProfileFilled(jobSeeker);

            Assert.True(result);
        }
        [Fact]
        public void IsJobSeekerProfileFilled_ShouldReturnFalse()
        {
            var jobSeeker = new JobSeeker
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                Experience = "too short string",
                Phone = string.Empty
            };

            bool result = _adminService.IsJobSeekerProfileFilled(jobSeeker);

            Assert.False(result);
        }
        [Fact]
        public void IsRecruiterProfileFilled_ShoulReturnTrue()
        {
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

            bool result = _adminService.IsRecuiterProfileFilled(recruiter);

            Assert.True(result);
        }
        [Fact]
        public void IsRecruiterProfileFilled_ShouldRetrunFalse()
        {
            var recruiter = new Recruiter
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                Company = string.Empty,
                CompanyDescription = "too short string"
            };

            bool result = _adminService.IsRecuiterProfileFilled(recruiter);

            Assert.False(result);
        }
    }
}
