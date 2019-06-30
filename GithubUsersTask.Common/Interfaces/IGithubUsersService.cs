using GithubUsersTask.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubUsersTask.Common.Interfaces
{
    /// <summary>
    /// interface for service to access github users api
    /// </summary>
    public interface IGithubUsersService
    {
        /// <summary>
        /// method to access github user information
        /// </summary>
        /// <param name="userName">parameter for username</param>
        /// <returns>returns github user information</returns>
        Task<GithubUser> GetUserAsync(string userName);

        /// <summary>
        /// method to access github user repositories
        /// </summary>
        /// <param name="userRepoUrl">parameter for user's specific repos_url</param>
        /// <returns>returns list of repository information for given user</returns>
        Task<IEnumerable<GithubUserRepo>> GetUserRepositoriesAsync(string userRepoUrl);
    }
}
