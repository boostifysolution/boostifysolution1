using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoostifySolution.Models.Users
{
    public class CurrentUserObj
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("AccountStatus")]
        public int AccountStatus { get; set; }
    }
}