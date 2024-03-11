using Newtonsoft.Json;

namespace BoostifySolution.Models.Global
{
    public class TooltipObject
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

    }
}
