using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace boostifysolution1.Models.Admin
{
    public class AdminLeadConvertToUserRequest
    {
        [JsonProperty("userLeadIds")]
        public List<int> UserLeadIds { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("adminStaffId")]
        public int AdminStaffId { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("walletAmount")]
        public decimal WalletAmount { get; set; }
    }
}