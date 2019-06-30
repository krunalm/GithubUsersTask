using GithubUsersTask.Common.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace GithubUsersTask.Framework
{
    /// <summary>
    /// Http client to access github api
    /// </summary>
    public class GithubApiClient : IGithubApiClient
    {
        private HttpClient _httpClient;
        private const string ClientUserAgent = "mvc-github-api-client"; // github api requires user agent
        private const string BaseUrl = "https://api.github.com";
        private const string MediaTypeHeader = "application/json";

        public GithubApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateHttpClient(BaseUrl, MediaTypeHeader, ClientUserAgent);
        }

        /// <summary>
        /// method to perform GET request for given url and returns response for specific type
        /// </summary>
        /// <typeparam name="TResult">generic return type</typeparam>
        /// <param name="url">request url</param>
        /// <returns>returns get request response deserialized to given generic type</returns>
        public async Task<TResult> GetAsync<TResult>(string url)
        {
            var response = await GetAsync(url);
            if (string.IsNullOrWhiteSpace(response))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<TResult>(response);
        }

        /// <summary>
        /// method to perform GET request for given url and returns string response
        /// </summary>
        /// <param name="url">request url</param>
        /// <returns>returns get request response as string</returns>
        public async Task<string> GetAsync(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}