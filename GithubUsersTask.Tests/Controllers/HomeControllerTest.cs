using GithubUsersTask.Common.Interfaces;
using GithubUsersTask.Common.Models;
using GithubUsersTask.Controllers;
using Moq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace GithubUsersTask.Tests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index()
        {
            // Arrange
            var githubUsersServiceMock = new Mock<IGithubUsersService>(MockBehavior.Strict);
            HomeController controller = new HomeController(githubUsersServiceMock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Index_Post_Returns_View_With_User()
        {
            // Arrange
            var username = "user";
            var userResult = new GithubUser
            {
                UserName = "user",
                Name = "User",
            };

            var githubUsersServiceMock = new Mock<IGithubUsersService>(MockBehavior.Strict);
            githubUsersServiceMock.Setup(service => service.GetUserAsync(username)).ReturnsAsync(userResult);

            HomeController controller = new HomeController(githubUsersServiceMock.Object);

            // Act
            ViewResult result = await controller.Index(username) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result.Model);
            Assert.IsType<GithubUser>(result.Model);
            var user = result.Model as GithubUser;
            Assert.Equal(user.UserName, userResult.UserName);
            Assert.Equal(user.Name, userResult.Name);
        }
    }
}
