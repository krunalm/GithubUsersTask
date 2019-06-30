using GithubUsersTask.Common.Interfaces;
using GithubUsersTask.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubUsersTask.Services
{
    /// <summary>
    /// Service to access github users api
    /// </summary>
    public class GithubUsersService : IGithubUsersService
    {
        private readonly IGithubApiClient _apiClient;
        private const string GithubUserAPIUrl = "https://api.github.com/users/{0}";

        public GithubUsersService(IGithubApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Gets github user for given user name
        /// </summary>
        /// <param name="userName">github username for request</param>
        /// <returns>returns github user and it's repository information</returns>
        public async Task<GithubUser> GetUserAsync(string userName)
        {
            var usersApiUrl = string.Format(GithubUserAPIUrl, userName);

            var user = await _apiClient.GetAsync<GithubUser>(usersApiUrl);
            if (user == null)
            {
                return null;
            }

            // get user's repositories using repos_url
            var userRepositories = await GetUserRepositoriesAsync(user.RepoUrl);

            // filter top 5 starred repository
            user.Repositories = userRepositories.OrderByDescending(r => r.Stars).Take(5);

            return user;
        }

        /// <summary>
        /// Gets users repository information
        /// </summary>
        /// <param name="userRepoUrl">github user repository url</param>
        /// <returns>list of github repository information</returns>
        public async Task<IEnumerable<GithubUserRepo>> GetUserRepositoriesAsync(string userRepoUrl)
        {
            return await _apiClient.GetAsync<IEnumerable<GithubUserRepo>>(userRepoUrl);
        }
    }
}