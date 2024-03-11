using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminUserDetails
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
        public string WalletAmount { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("showPopupMessage")]
        public bool ShowPopupMessage { get; set; }

        [JsonProperty("popupMessage")]
        public string PopupMessage { get; set; }

        [JsonProperty("popupMessageTitle")]
        public string PopupMessageTitle { get; set; }

        [JsonProperty("walletTransactionList")]
        public List<WalletTransactionListDetails> WalletTransactionList { get; set; }

        [JsonProperty("userTaskList")]
        public List<UserTaskListDetail> UserTaskList { get; set; }

    }

    public class WalletTransactionListDetails
    {
        [JsonProperty("walletTransactionId")]
        public int WalletTransactionId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }
    }

    public class UserTaskListDetail
    {
        [JsonProperty("userTaskId")]
        public int UserTaskId { get; set; }

        [JsonProperty("taskTitle")]
        public string TaskTitle { get; set; }

        [JsonProperty("commission")]
        public string Commission { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("quantityPurchased")]
        public string QuantityPurchased { get; set; }

        [JsonProperty("dateCompleted")]
        public string DateCompleted { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }
    }
}