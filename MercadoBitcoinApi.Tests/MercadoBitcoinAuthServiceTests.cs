using System.Net;
using System.Net.Http.Json;
using MercadoBitcoinApi.Models;
using MercadoBitcoinApi.Services;
using Moq;
using Moq.Protected;

namespace MercadoBitcoinApi.Tests;

public class MercadoBitcoinServiceTests
{
    private static HttpClient CreateHttpClient(Mock<HttpMessageHandler> handler)
    {
        var client = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://api.mercadobitcoin.net")
        };
        return client;
    }

    [Fact]
    public async Task AuthorizeAsync_ReturnsAuthResponse_WhenCredentialsAreValid()
    {
        var authResponse = new AuthResponse
        {
            Access_Token = "token123",
            Expiration = 3600
        };

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
                Content = JsonContent.Create(new { access_token = authResponse.Access_Token, expiration = authResponse.Expiration })
            });

        var httpClient = CreateHttpClient(handler);
        var service = new MercadoBitcoinService(httpClient);

        var result = await service.AuthorizeAsync("login", "password");

        Assert.NotNull(result);
        Assert.Equal(authResponse.Access_Token, result.Access_Token);
        Assert.Equal(authResponse.Expiration, result.Expiration);
    }

    [Fact]
    public async Task GetAccountsAsync_ReturnsList_WhenAuthorized()
    {
        var accountsJson = new[]
        {
            new { id = "1", name = "acc1" },
            new { id = "2", name = "acc2" }
        };

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath == "/api/v4/accounts" &&
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "token123"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(accountsJson)
            });

        var httpClient = CreateHttpClient(handler);
        var service = new MercadoBitcoinService(httpClient);

        var list = await service.GetAccountsAsync("token123", CancellationToken.None);

        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetPositionsAsync_ReturnsList_WithQueryParameters()
    {
        var positionsJson = new[]
        {
            new { asset = "BTC", quantity = 0.5 },
            new { asset = "ETH", quantity = 2.0 }
        };

        var accountId = "account-abc";
        var start = new DateTime(2023, 01, 01);
        var end = new DateTime(2023, 01, 31);

        var expectedPath = $"/api/v4/accounts/{accountId}/positions";
        var expectedQuery = $"?start_date={start:yyyy-MM-dd}&end_date={end:yyyy-MM-dd}";

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath == expectedPath &&
                    req.RequestUri.Query.Contains("start_date=2023-01-01") &&
                    req.RequestUri.Query.Contains("end_date=2023-01-31") &&
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Parameter == "token123"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(positionsJson)
            });

        var httpClient = CreateHttpClient(handler);
        var service = new MercadoBitcoinService(httpClient);

        var list = await service.GetPositionsAsync("token123", accountId, start, end, CancellationToken.None);

        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetAccountsAsync_ThrowsArgumentException_WhenAccessTokenIsEmpty()
    {
        var handler = new Mock<HttpMessageHandler>();
        var httpClient = CreateHttpClient(handler);
        var service = new MercadoBitcoinService(httpClient);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetAccountsAsync(string.Empty, CancellationToken.None));
    }
}