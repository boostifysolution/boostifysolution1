using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminNewUserDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("loginUsername")]
        public string LoginUsername { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

         [JsonProperty("country")]
        public int Country { get; set; }

        [JsonProperty("walletAmount")]
        public decimal WalletAmount { get; set; }
    }
}