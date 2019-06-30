using GithubUsersTask.Common.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GithubUsersTask.Framework
{
    /// <summary>
    /// factory class to create httpclient instances
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// creates httpclient with given configurations
        /// </summary>
        public HttpClient CreateHttpClient(string baseUrl, string mediaTypeHeader, string userAgent)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            // specify to use TLS 1.2 as default connection
            // without this, accessing https github api url was throwing error for ssl connection
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeHeader));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            return httpClient;
        }
    }
}