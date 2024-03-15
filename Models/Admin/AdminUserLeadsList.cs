using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminUserLeadsList
    {
        [JsonProperty("userLeadsList")]
        public List<AdminUserLeadsListDetails> UserLeadsList { get; set; }

        [JsonProperty("itemsCount")]
        public int ItemsCount { get; set; }

        [JsonProperty("leadStatusOptions")]
        public List<DropdownOptions> LeadStatusOptions { get; set; }

        [JsonProperty("dateAddedOptions")]
        public List<DropdownOptions> DateAddedOptions { get; set; }

        [JsonProperty("adminOptions")]
        public List<DropdownOptions> AdminOptions { get; set; }

        [JsonProperty("languageOptions")]
        public List<DropdownOptions> LanguageOptions { get; set; }
    }

    public class AdminUserLeadsListDetails
    {
        [JsonProperty("userLeadId")]
        public int UserLeadId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("leadStatus")]
        public string LeadStatus { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}