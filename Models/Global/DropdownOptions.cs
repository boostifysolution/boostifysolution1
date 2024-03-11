using Newtonsoft.Json;

namespace BoostifySolution.Models.Global
{
    public class DropdownOptions
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
