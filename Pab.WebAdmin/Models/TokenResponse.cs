using Newtonsoft.Json;

namespace Pab.WebAdmin.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
    }
}
