using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminAddCreditDetails
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("walletTransactionType")]
        public int WalletTransactionType { get; set; }

         [JsonProperty("customDateAdded")]
        public DateTime? CustomDateAdded { get; set; }
    }
}