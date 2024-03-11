using Newtonsoft.Json;

namespace BoostifySolution.Models.Global
{
    public class AuthenticationResult
    {
        [JsonProperty("errors")]
        public IEnumerable<string> Errors { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("entityId")]
        public int EntityID { get; set; }

        [JsonProperty("newEntity")]
        public bool NewEntity { get; set; }

        [JsonProperty("extraInfo")]
        public string ExtraInfo { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("requirePasswordChange")]
        public bool RequirePasswordChange { get; set; }

    }
}
