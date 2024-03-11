using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminUsersList
    {
        [JsonProperty("usersList")]
        public List<AdminUserListDetails> UsersList { get; set; }

        [JsonProperty("itemsCount")]
        public int ItemsCount { get; set; }

        [JsonProperty("dateFilterOptions")]
        public List<DropdownOptions> DateFilterOptions { get; set; }

        [JsonProperty("accountStatusOptions")]
        public List<DropdownOptions> AccountStatusOptions { get; set; }

        [JsonProperty("countryOptions")]
        public List<DropdownOptions> CountryOptions { get; set; }

        [JsonProperty("languageOptions")]
        public List<DropdownOptions> LanguageOptions { get; set; }

        [JsonProperty("currencyOptions")]
        public List<DropdownOptions> CurrencyOptions { get; set; }
    }

    public class AdminUserListDetails
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("wallet")]
        public string Wallet { get; set; }

        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("adminStaffName")]
        public string AdminStaffName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}