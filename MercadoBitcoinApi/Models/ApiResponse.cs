using Newtonsoft.Json;

namespace MercadoBitcoinApi.Models;

/// <summary>
/// Resposta genérica da API
/// </summary>
public class ApiResponse<T>
{
    [JsonProperty("data")]
    public T? Data { get; set; }

    [JsonProperty("error")]
    public string? Error { get; set; }

    public bool IsSuccess => string.IsNullOrEmpty(Error);
}

