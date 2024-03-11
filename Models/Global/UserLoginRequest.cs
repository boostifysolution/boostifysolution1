using Newtonsoft.Json;

namespace BoostifySolution.Models.Global
{
    public class UserLoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

