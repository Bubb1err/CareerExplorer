using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Services;
using Moq;

namespace CareerExplorer.Tests.Services.Tests
{
    public class VacancyServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly VacancyService _vacancyService;
        public VacancyServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _vacancyService = new VacancyService(_unitOfWorkMock.Object);
        }
        [Fact]
        public void GetIdsFromString_ShouldReturnNotNullArray()
        {
            var testString = "1,2,3,4";

            int[]? result = _vacancyService.GetIdsFromString(testString);

            Assert.NotNull(result);
            Assert.Equal(new int[] { 1, 2, 3, 4 }, result);
        }
        [Fact]
        public void GetIdsFromString_ShouldBeNull()
        {
            string testString = string.Empty;

            int[]? result = _vacancyService.GetIdsFromString(testString);

            Assert.Null(result);
        }
        [Fact]
        public void GetIdsFromString_ShouldThrowAnException()
        {
            string testString = "a,dk,ssk";

            Action action = () => _vacancyService.GetIdsFromString(testString);

            Assert.Throws<ArgumentException>(action);
        }
    }
}
