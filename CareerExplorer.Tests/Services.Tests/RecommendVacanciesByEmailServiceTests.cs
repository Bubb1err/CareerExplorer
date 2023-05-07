using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Identity.UI.Services;
using CareerExplorer.Infrastructure.Services;
using System.Linq.Expressions;

namespace CareerExplorer.Tests.Services.Tests
{
    public class RecommendVacanciesByEmailServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVacanciesRepository> _vacanciesRepositoryMock;
        private readonly Mock<IJobSeekerProfileRepository> _jobSeekerProfileRepositoryMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ILogger<RecommendVacanciesByEmailService>> _loggerMock;
        private readonly RecommendVacanciesByEmailService _service;

        public RecommendVacanciesByEmailServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _vacanciesRepositoryMock = new Mock<IVacanciesRepository>();
            _jobSeekerProfileRepositoryMock = new Mock<IJobSeekerProfileRepository>();
            _emailSenderMock = new Mock<IEmailSender>();
            _loggerMock = new Mock<ILogger<RecommendVacanciesByEmailService>>();

            _unitOfWorkMock.Setup(x => x.GetRepository<Vacancy>()).Returns(_vacanciesRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.GetRepository<JobSeeker>()).Returns(_jobSeekerProfileRepositoryMock.Object);

            _service = new RecommendVacanciesByEmailService(
                _unitOfWorkMock.Object,
                _emailSenderMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task SendVacanciesToUsersByEmail_ShouldSendEmails_WhenVacanciesMatchDesiredPosition()
        {
            var checkingPeriod = TimeSpan.FromDays(1);
            DateTime time = DateTime.UtcNow - checkingPeriod;
            var position = new Position
            {
                Name = "Test"
            };
            var vacancy = new Vacancy
            {
                Id = 1,
                Position = position,
                Creator = new Recruiter { Company = "Test" },
                CreatedDate = DateTime.UtcNow
            };
            var jobSeeker = new JobSeeker
            {
                Id = 1,
                DesiredPosition = position,
                IsSubscribedToNotification = true,
                AppUser = new AppUser { Email = "test@test.com" }
            };
            _vacanciesRepositoryMock.Setup(x => x.GetAll
            (It.IsAny<Expression<Func<Vacancy, bool>>>(), It.IsAny<string>()))
                .Returns(new List<Vacancy> { vacancy });
            _jobSeekerProfileRepositoryMock.Setup(x => x.GetAll
            (It.IsAny<Expression<Func<JobSeeker, bool>>>(), It.IsAny<string>()))
                .Returns(new List<JobSeeker> { jobSeeker });

            await _service.SendVacanciesToUsersByEmail(checkingPeriod);

            _emailSenderMock.Verify(x => x.SendEmailAsync(jobSeeker.AppUser.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async Task SendVacanciesToUsersByEmail_ShouldNotSendEmails_WhenVacanciesDoNotMatchDesiredPosition()
        {
            var checkingPeriod = TimeSpan.FromDays(1);
            DateTime time = DateTime.UtcNow - checkingPeriod;
            var vacancy = new Vacancy
            {
                Id = 1,
                Position = new Position { Name = "Test"},
                Creator = new Recruiter { Company = "Test" },
                CreatedDate = DateTime.UtcNow
            };
            var jobSeeker = new JobSeeker
            {
                Id = 1,
                DesiredPosition = new Position { Name = "Test"},
                IsSubscribedToNotification = true,
                AppUser = new AppUser { Email = "test@test.com" }
            };
            _vacanciesRepositoryMock.Setup(x => x.GetAll
            (It.IsAny<Expression<Func<Vacancy, bool>>>(), It.IsAny<string>()))
                .Returns(new List<Vacancy> { vacancy });
            _jobSeekerProfileRepositoryMock.Setup(x => x.GetAll
            (It.IsAny<Expression<Func<JobSeeker, bool>>>(), It.IsAny<string>()))
                .Returns(new List<JobSeeker> { jobSeeker });

            await _service.SendVacanciesToUsersByEmail(checkingPeriod);

            _emailSenderMock.Verify
                (sender => sender.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
