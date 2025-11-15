using MercadoBitcoinApi.Models;

namespace MercadoBitcoinApi.Services.Interfaces;

/// <summary>
/// Interface para serviço de autenticação
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Gera autenticação para requisições à API
    /// </summary>
    /// <param name="method">Método HTTP (GET, POST, etc.)</param>
    /// <param name="path">Caminho da requisição</param>
    /// <param name="queryString">Query string da requisição</param>
    /// <param name="body">Corpo da requisição (se houver)</param>
    /// <returns>Dicionário com os cabeçalhos de autenticação</returns>
    Task<AuthResponse> AuthenticateAsync(CancellationToken ct = default);

}

