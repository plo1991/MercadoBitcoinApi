using Microsoft.AspNetCore.Mvc;
using MercadoBitcoinApi.Models;
using MercadoBitcoinApi.Services;
using System.ComponentModel.DataAnnotations;
using MercadoBitcoinApi.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MercadoBitcoinApi.Controllers;

/// <summary>
/// Controller para integração com a API do Mercado Bitcoin
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MercadoBitcoinController : ControllerBase
{
    private readonly IMercadoBitcoinService _mercadoBitcoinService;
    private readonly IAuthenticationService _authenticationService; 
    private readonly ILogger<MercadoBitcoinController> _logger;

    public MercadoBitcoinController(
        IMercadoBitcoinService mercadoBitcoinService,
        IAuthenticationService authenticationService,
        ILogger<MercadoBitcoinController> logger)
    {
        _mercadoBitcoinService = mercadoBitcoinService;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as contas do usuário
    /// </summary>
    /// <returns>Lista de contas do usuário</returns>
    /// <response code="200">Retorna a lista de contas com sucesso</response>
    /// <response code="401">Erro de autenticação com a API do Mercado Bitcoin</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("accounts")]
    [ProducesResponseType(typeof(List<Account>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Account>>> GetAccounts(CancellationToken ct)
    {
        try
        {
            var auth = await _authenticationService.AuthenticateAsync();
            var accounts = await _mercadoBitcoinService.GetAccountsAsync(auth.Access_Token!, ct);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar contas");
            return StatusCode(500, new { error = "Erro ao consultar contas", message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém as posições de uma conta específica
    /// </summary>
    /// <param name="accountId">ID da conta</param>
    /// <param name="startDate">Data inicial para filtro (opcional). Formato: yyyy-MM-dd</param>
    /// <param name="endDate">Data final para filtro (opcional). Formato: yyyy-MM-dd</param>
    /// <returns>Lista de posições da conta</returns>
    /// <response code="200">Retorna a lista de posições com sucesso</response>
    /// <response code="400">Parâmetros inválidos (accountId vazio ou datas inconsistentes)</response>
    /// <response code="401">Erro de autenticação com a API do Mercado Bitcoin</response>
    /// <response code="404">Conta não encontrada</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("accounts/{accountId}/positions")]
    [ProducesResponseType(typeof(List<Position>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Position>>> GetPositions(
        CancellationToken ct,
        [Required] string accountId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var auth = await _authenticationService.AuthenticateAsync();
            var positions = await _mercadoBitcoinService.GetPositionsAsync(auth.Access_Token!, accountId, startDate, endDate, ct);
            return Ok(positions);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Parâmetros inválidos para consulta de posições");
            return BadRequest(new { error = "Parâmetros inválidos", message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar posições para a conta {AccountId}", accountId);
            return StatusCode(500, new { error = "Erro ao consultar posições", message = ex.Message });
        }
    }

    /// <summary>
    /// Encaminha uma requisição de autorização para a API do Mercado Bitcoin (POST /authorize).
    /// Retorna o JSON bruto recebido da API (ex.: access_token, expiration, etc).
    /// </summary>
    [HttpPost("authorize")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authorize([FromBody] AuthorizeRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { error = "Login e password são obrigatórios" });

        try
        {
            AuthResponse result = await _mercadoBitcoinService.AuthorizeAsync(request.Login, request.Password);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha na requisição de autorização");
            return StatusCode(500, new { error = "Falha na requisição de autorização", message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao autorizar");
            return StatusCode(500, new { error = "Erro ao autorizar", message = ex.Message });
        }
    }
}