using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserWithdrawWalletDetails
    {
        [JsonProperty("withdrawAmount")]
        public decimal WithdrawAmount { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("accountHolderName")]
        public string AccountHolderName { get; set; }

        [JsonProperty("bankAccountNumber")]
        public string BankAccountNumber { get; set; }


        [JsonProperty("isfcCode")]
        public string ISFCCode { get; set; }
    }
}