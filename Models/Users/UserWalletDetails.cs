using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserWalletDetails
    {
        [JsonProperty("walletAmount")]
        public string WalletAmount { get; set; }

        [JsonProperty("walletAmountDecimal")]
        public decimal WalletAmountDecimal { get; set; }

        [JsonProperty("walletLastUpdated")]
        public string WalletLastUpdated { get; set; }

        [JsonProperty("walletTransactions")]
        public List<WalletTransactionListDetails> WalletTransactions { get; set; }

        [JsonProperty("showPopupMessage")]
        public bool ShowPopupMessage { get; set; }

        [JsonProperty("popupMessageTitle")]
        public string? PopupMessageTitle { get; set; }

        [JsonProperty("popupMessage")]
        public string? PopupMessage { get; set; }

        [JsonProperty("accountStatus")]
        public int AccountStatus { get; set; }
    }
}