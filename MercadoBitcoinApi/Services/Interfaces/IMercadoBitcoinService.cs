using MercadoBitcoinApi.Models;
using Newtonsoft.Json.Linq;

namespace MercadoBitcoinApi.Services.Interfaces;

/// <summary>
/// Interface para o serviço de integração com a API do Mercado Bitcoin
/// </summary>
public interface IMercadoBitcoinService
{
    /// <summary>
    /// Obtém todas as contas do usuário
    /// </summary>
    Task<List<Account>> GetAccountsAsync(string accessToken, CancellationToken ct = default);

    /// <summary>
    /// Obtém as posições de uma conta específica
    /// </summary>
    /// <param name="accessToken">Token</param>
    /// <param name="ct">Cancellation Token</param>
    /// <param name="accountId">ID da conta</param>
    /// <param name="startDate">Data inicial para filtro (opcional)</param>
    /// <param name="endDate">Data final para filtro (opcional)</param>
    Task<List<Position>> GetPositionsAsync(string accessToken, string accountId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    /// <summary>
    /// Autoriza um usuário (endpoint /authorize) e retorna o JSON bruto da resposta como JObject.
    /// Uso: POST /api/v4/authorize com { login, password } no body.
    /// </summary>
    Task<AuthResponse> AuthorizeAsync(string login, string password);
}