using System.Net.Http;

namespace GithubUsersTask.Common.Interfaces
{
    /// <summary>
    /// interface for httpclient factory 
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates httpclient object based on given configuration
        /// </summary>
        /// <param name="baseUrl">base url to be used for httpclient</param>
        /// <param name="mediaTypeHeader">media type header for httpclient</param>
        /// <param name="userAgent">user agent header for httpclient</param>
        /// <returns>HttpClient object</returns>
        HttpClient CreateHttpClient(string baseUrl, string mediaTypeHeader, string userAgent);
    }
}
