using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using MercadoBitcoinApi.Models;
using MercadoBitcoinApi.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace MercadoBitcoinApi.Tests;

public class AuthenticationServiceTests
{
    private static HttpClient CreateHttpClient(Mock<HttpMessageHandler> handler)
    {
        return new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://api.mercadobitcoin.net")
        };
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsAuthResponse_WhenApiReturnsToken()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath == "/api/v4/authorize"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { access_token = "abc123", expiration = 3600 })
            });

        var httpClient = CreateHttpClient(handler);
        var service = new AuthenticationService(httpClient, "tapiId", "tapiSecret");

        var result = await service.AuthenticateAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("abc123", result.Access_Token);
        Assert.Equal(3600, result.Expiration);
    }

    [Fact]
    public async Task AuthenticateAsync_ThrowsInvalidOperationException_WhenResponseHasNoAccessToken()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { some_field = "value" })
            });

        var httpClient = CreateHttpClient(handler);
        var service = new AuthenticationService(httpClient, "tapiId", "tapiSecret");

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AuthenticateAsync(CancellationToken.None));
    }

    [Fact]
    public async Task AuthenticateAsync_ThrowsHttpRequestException_WhenApiReturnsNonSuccessStatus()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = JsonContent.Create(new { error = "unauthorized" })
            });

        var httpClient = CreateHttpClient(handler);
        var service = new AuthenticationService(httpClient, "tapiId", "tapiSecret");

        await Assert.ThrowsAsync<HttpRequestException>(() => service.AuthenticateAsync(CancellationToken.None));
    }
}