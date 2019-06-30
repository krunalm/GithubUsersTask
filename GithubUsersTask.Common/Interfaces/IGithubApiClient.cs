using System.Threading.Tasks;

namespace GithubUsersTask.Common.Interfaces
{
    /// <summary>
    /// interface for github api client
    /// </summary>
    public interface IGithubApiClient
    {
        /// <summary>
        /// method to perform get request and returns result deserialized to given type
        /// </summary>
        /// <typeparam name="TResult">generic type in which json response to be deserialized</typeparam>
        /// <param name="url">request url</param>
        /// <returns>returns get request response deserialized to type given</returns>
        Task<TResult> GetAsync<TResult>(string url);

        /// <summary>
        /// method to perform get request and returns result in string
        /// </summary>
        /// <param name="url">request url</param>
        /// <returns>returns get request response in string</returns>
        Task<string> GetAsync(string url);
    }
}
