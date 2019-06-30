using GithubUsersTask.Common.Interfaces;
using GithubUsersTask.Common.Models;
using GithubUsersTask.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GithubUsersTask.Tests.Services
{
    public class GithubUsersServiceTests
    {
        [Fact]
        public async Task GetUserAsync_Success_Returns_User()
        {
            // Arrange
            var username = "user";
            var githubUserUrl = string.Format("https://api.github.com/users/{0}", username);
            var githubUserRepoUrl = string.Format("https://api.github.com/users/{0}/repos", username);
            var userResult = new GithubUser
            {
                UserName = "user",
                Name = "User",
                RepoUrl = githubUserRepoUrl
            };

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<GithubUser>(githubUserUrl)).ReturnsAsync(userResult);
            githubApiClientMock.Setup(client => client.GetAsync<IEnumerable<GithubUserRepo>>(githubUserRepoUrl)).ReturnsAsync(new List<GithubUserRepo>());

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act
            GithubUser result = await githubUsersService.GetUserAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GithubUser>(result);
            var user = result as GithubUser;
            Assert.Equal(user.UserName, userResult.UserName);
            Assert.Equal(user.Name, userResult.Name);
        }

        [Fact]
        public async Task GetUserAsync_Success_Returns_User_With_Top5Repositories()
        {
            // Arrange
            var username = "user";
            var githubUserUrl = string.Format("https://api.github.com/users/{0}", username);
            var githubUserRepoUrl = string.Format("https://api.github.com/users/{0}/repos", username);

            var userRepos = new List<GithubUserRepo>
            {
                new GithubUserRepo
                {
                    RepositoryName = "one",
                    Stars = 0
                },
                new GithubUserRepo
                {
                    RepositoryName = "two",
                    Stars = 1
                },
                new GithubUserRepo
                {
                    RepositoryName = "three",
                    Stars = 5
                },
                new GithubUserRepo
                {
                    RepositoryName = "four",
                    Stars = 3
                },
                new GithubUserRepo
                {
                    RepositoryName = "five",
                    Stars = 10
                },
                new GithubUserRepo
                {
                    RepositoryName = "six",
                    Stars = 8
                }
            };
            var userResult = new GithubUser
            {
                UserName = "user",
                Name = "User",
                RepoUrl = githubUserRepoUrl
            };

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<GithubUser>(githubUserUrl)).ReturnsAsync(userResult);
            githubApiClientMock.Setup(client => client.GetAsync<IEnumerable<GithubUserRepo>>(githubUserRepoUrl)).ReturnsAsync(userRepos);

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act
            GithubUser result = await githubUsersService.GetUserAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GithubUser>(result);

            var user = result as GithubUser;
            Assert.Equal(user.UserName, userResult.UserName);
            Assert.Equal(user.Name, userResult.Name);

            Assert.IsAssignableFrom<IEnumerable<GithubUserRepo>>(user.Repositories);
            var repos = user.Repositories.ToArray();
            Assert.Equal(5, repos.Length);
            Assert.Equal(userRepos[4].RepositoryName, repos[0].RepositoryName);
            Assert.Equal(userRepos[4].Stars, repos[0].Stars);
            Assert.Equal(userRepos[5].RepositoryName, repos[1].RepositoryName);
            Assert.Equal(userRepos[5].Stars, repos[1].Stars);
            Assert.Equal(userRepos[2].RepositoryName, repos[2].RepositoryName);
            Assert.Equal(userRepos[2].Stars, repos[2].Stars);
            Assert.Equal(userRepos[3].RepositoryName, repos[3].RepositoryName);
            Assert.Equal(userRepos[3].Stars, repos[3].Stars);
            Assert.Equal(userRepos[1].RepositoryName, repos[4].RepositoryName);
            Assert.Equal(userRepos[1].Stars, repos[4].Stars);
        }

        [Fact]
        public async Task GetUserAsync_Success_Returns_Null_WhenUserNotFound()
        {
            // Arrange
            var username = "user";
            var githubUserUrl = string.Format("https://api.github.com/users/{0}", username);

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<GithubUser>(githubUserUrl)).ReturnsAsync((GithubUser)null);

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act
            GithubUser result = await githubUsersService.GetUserAsync(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserAsync_Fail_ThrowsException()
        {
            // Arrange
            var username = "user";
            var githubUserUrl = string.Format("https://api.github.com/users/{0}", username);

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<GithubUser>(githubUserUrl)).Throws(new HttpRequestException());

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await githubUsersService.GetUserAsync(username);
            });
        }

        [Fact]
        public async Task GetUserRepositoriesAsync_Success_Returns_UserRepos()
        {
            // Arrange
            var githubUserRepoUrl = "https://api.github.com/users/test/repos";
            var userReposResult = new List<GithubUserRepo>
            {
                new GithubUserRepo
                {
                    RepositoryName = "test",
                    Stars = 1
                }
            };

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<IEnumerable<GithubUserRepo>>(githubUserRepoUrl)).ReturnsAsync(userReposResult);

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act
            var result = await githubUsersService.GetUserRepositoriesAsync(githubUserRepoUrl);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<GithubUserRepo>>(result);
            var userRepo = (result as IEnumerable<GithubUserRepo>).First();
            Assert.Equal(userReposResult.First().Stars, userRepo.Stars);
            Assert.Equal(userReposResult.First().RepositoryName, userRepo.RepositoryName);
        }

        [Fact]
        public async Task GetUserRepositoriesAsync_Fail_ThrowsException()
        {
            // Arrange
            var githubUserRepoUrl = "https://api.github.com/users/test/repos";

            var githubApiClientMock = new Mock<IGithubApiClient>(MockBehavior.Strict);
            githubApiClientMock.Setup(client => client.GetAsync<IEnumerable<GithubUserRepo>>(githubUserRepoUrl)).Throws(new HttpRequestException());

            GithubUsersService githubUsersService = new GithubUsersService(githubApiClientMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await githubUsersService.GetUserRepositoriesAsync(githubUserRepoUrl);
            });
        }
    }
}
