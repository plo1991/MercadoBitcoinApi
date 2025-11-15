using Newtonsoft.Json;

namespace MercadoBitcoinApi.Models;

/// <summary>
/// Representa uma conta do Mercado Bitcoin
/// </summary>
public class Account
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonProperty("currencySign")]
    public string CurrencySign { get; set; } = string.Empty;
}

