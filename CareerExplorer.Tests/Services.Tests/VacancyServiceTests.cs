using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Services;
using Moq;

namespace CareerExplorer.Tests.Services.Tests
{
    public class VacancyServiceTests
    {
        [Fact]
        public void GetIdsFromString_ShouldReturnNotNullArray()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var testString = "1,2,3,4";
            var vacancyService = new VacancyService(unitOfWork.Object);

            int[]? result = vacancyService.GetIdsFromString(testString);

            Assert.NotNull(result);
            Assert.Equal(new int[] { 1, 2, 3, 4 }, result);
        }
        [Fact]
        public void GetIdsFromString_ShouldBeNull()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var vacancyService = new VacancyService(unitOfWork.Object);

            int[]? result = vacancyService.GetIdsFromString(string.Empty);

            Assert.Null(result);
        }
        [Fact]
        public void GetIdsFromString_ShouldThrowAnException()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var vacancyService = new VacancyService(unitOfWork.Object);
            string testString = "a,dk,ssk";

            Action action = () => vacancyService.GetIdsFromString(testString);

            Assert.Throws<ArgumentException>(action);
        }
    }
}
