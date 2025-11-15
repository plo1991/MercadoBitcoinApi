using MercadoBitcoinApi.Models;
using MercadoBitcoinApi.Services.Interfaces;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace MercadoBitcoinApi.Services;

/// <summary>
/// Serviço responsável pela autenticação HMAC-SHA512 para a API do Mercado Bitcoin
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _http;
    private readonly string _tapiId;
    private readonly string _tapiSecret;

    public AuthenticationService(HttpClient http, string tapiId, string tapiSecret)
    {
        _http = http;
        _tapiId = tapiId ?? throw new ArgumentNullException(nameof(tapiId));
        _tapiSecret = tapiSecret ?? throw new ArgumentNullException(nameof(tapiSecret));
    }

    /// <summary>
    /// Gera Autenticação Token 
    /// Bearer Token
    /// </summary>
    public async Task<AuthResponse> AuthenticateAsync(CancellationToken ct = default)
    {
        var login = _tapiId ?? throw new InvalidOperationException("ApiTokenId is not configured");
        var password = _tapiSecret ?? throw new InvalidOperationException("ApiTokenSecret is not configured");

        var request = new AuthorizeRequest(login, password);

        var resp = await _http.PostAsJsonAsync("/api/v4/authorize", request, ct);
        resp.EnsureSuccessStatusCode();

        var auth = await resp.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct);
        if (auth == null || string.IsNullOrEmpty(auth.Access_Token))
            throw new InvalidOperationException("Authentication failed or returned no access_token");

        return auth;
    }
}

