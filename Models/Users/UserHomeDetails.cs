using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserHomeDetails
    {
        [JsonProperty("userTasks")]
        public List<UserTasksListDetails> UserTasks { get; set; }

        [JsonProperty("completedUserTasks")]
        public List<UserTasksListDetails> CompletedUserTasks { get; set; }

        [JsonProperty("completuserSubmittedTasksedUserTasks")]
        public List<UserTasksListDetails> UserSubmittedTasks { get; set; }

        [JsonProperty("walletAmount")]
        public string WalletAmount { get; set; }

        [JsonProperty("walletLastUpdated")]
        public string WalletLastUpdated { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("accountStatus")]
        public int AccountStatus { get; set; }

        [JsonProperty("accountStatusMessage")]
        public string AccountStatusMessage { get; set; }
    }

    public class UserTasksListDetails
    {
        [JsonProperty("userTaskId")]
        public int UserTaskId { get; set; }

        [JsonProperty("platform")]
        public int Platform { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("taskTitle")]
        public string TaskTitle { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("taskAmount")]
        public string TaskAmount { get; set; }
    }

}