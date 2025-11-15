using Newtonsoft.Json;

namespace MercadoBitcoinApi.Models;

public class AuthResponse
{
    [JsonProperty("access_token")]
    public string Access_Token { get; set; } = string.Empty;
    
    [JsonProperty("expiration")]
    public int Expiration { get; set; }
}
