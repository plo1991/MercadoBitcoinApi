using MercadoBitcoinApi.Models;
using MercadoBitcoinApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MercadoBitcoinApi.Services;

public class MercadoBitcoinService : IMercadoBitcoinService
{
    private readonly HttpClient _http;

    public MercadoBitcoinService(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
    }

    public async Task<List<Account>> GetAccountsAsync(string accessToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("accessToken obrigatório", nameof(accessToken));

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v4/accounts");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _http.SendAsync(request, ct);
        var content = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Erro ao consultar contas. Status: {response.StatusCode}. Resposta: {content}");

        var list = JsonConvert.DeserializeObject<List<Account>>(content) ?? new List<Account>();
        return list;
    }

    public async Task<List<Position>> GetPositionsAsync(string accessToken, string accountId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("accessToken obrigatório", nameof(accessToken));
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException("AccountId não pode ser vazio", nameof(accountId));
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            throw new ArgumentException("A data inicial não pode ser posterior à data final", nameof(startDate));

        var url = $"/api/v4/accounts/{accountId}/positions";
        var query = new List<string>();
        if (startDate.HasValue) query.Add($"start_date={startDate.Value:yyyy-MM-dd}");
        if (endDate.HasValue) query.Add($"end_date={endDate.Value:yyyy-MM-dd}");
        if (query.Any()) url += "?" + string.Join("&", query);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _http.SendAsync(request, ct);
        var content = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Erro ao consultar posições. Status: {response.StatusCode}. Resposta: {content}");

        var list = JsonConvert.DeserializeObject<List<Position>>(content) ?? new List<Position>();
        return list;
    }

    public async Task<AuthResponse> AuthorizeAsync(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login não pode ser vazio", nameof(login));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password não pode ser vazio", nameof(password));

        var body = JsonConvert.SerializeObject(new { login, password });
        using var content = new StringContent(body, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v4/authorize") { Content = content };
        var response = await _http.SendAsync(request);
        var respString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Erro ao autorizar. Status: {response.StatusCode}. Resposta: {respString}");

        var auth = JsonConvert.DeserializeObject<AuthResponse>(respString)
                   ?? throw new InvalidOperationException("Resposta de autorização inválida");

        if (string.IsNullOrWhiteSpace(auth.Access_Token))
            throw new InvalidOperationException("Authorization response doesn't contain access_token");

        return auth;
    }
}