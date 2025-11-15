using Newtonsoft.Json;

namespace MercadoBitcoinApi.Models;

/// <summary>
/// Representa uma posição de ativo na conta
/// </summary>
public class Position
{
    [JsonProperty("avgPrice")]
    public decimal? AvgPrice { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; } = string.Empty;

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("instrument")]
    public string Instrument { get; set; }

    [JsonProperty("qty")]
    public string Qty { get; set; }

    [JsonProperty("side")]
    public string Side { get; set; }
}

