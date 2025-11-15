using System.ComponentModel.DataAnnotations;

namespace MercadoBitcoinApi.Models;

/// <summary>
/// Modelo usado para enviar login/password ao endpoint /api/mercadobitcoin/authorize
/// </summary>
public class AuthorizeRequest
{
    public AuthorizeRequest(string login, string password)
    {
        Login = login;
        Password = password;
    }

    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}