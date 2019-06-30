using GithubUsersTask.Common.Interfaces;
using GithubUsersTask.Common.Models;
using GithubUsersTask.Framework;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GithubUsersTask.Tests.Framework
{
    public class GithubApiClientTests
    {
        [Fact]
        public async Task GetAsync_String_Success_Returns_Result()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("content")
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act
            string result = await githubApiClient.GetAsync(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("content", result);
        }

        [Fact]
        public async Task GetAsync_String_Success_Returns_Null_WhenNoSuccessCode()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act
            string result = await githubApiClient.GetAsync(url);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_String_Fail_ThrowsException()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Throws<HttpRequestException>();

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await githubApiClient.GetAsync(url);
            });
        }

        [Fact]
        public async Task GetAsync_Object_Success_Returns_Result()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";
            var responseJson = "{\"login\":\"test\"}";
            var userResult = new GithubUser
            {
                UserName = "test"
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act
            GithubUser result = await githubApiClient.GetAsync<GithubUser>(url);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GithubUser>(result);
            Assert.Equal(userResult.UserName, result.UserName);
        }

        [Fact]
        public async Task GetAsync_Object_Success_Returns_Default_WhenNoResult()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act
            GithubUser result = await githubApiClient.GetAsync<GithubUser>(url);

            // Assert
            Assert.Equal(default, result);
        }

        [Fact]
        public async Task GetAsync_Object_Fail_ThrowsException()
        {
            // Arrange
            var url = "https://api.github.com/users/test";
            var baseUrl = "https://api.github.com";
            var mediaTypeHeader = "application/json";
            var userAgent = "mvc-github-api-client";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Throws<HttpRequestException>();

            var httpClient = new HttpClient(mockMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpClientFactoryMock.Setup(f => f.CreateHttpClient(baseUrl, mediaTypeHeader, userAgent)).Returns(httpClient);
            GithubApiClient githubApiClient = new GithubApiClient(httpClientFactoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await githubApiClient.GetAsync<GithubUser>(url);
            });
        }
    }
}
