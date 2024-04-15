using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminStaffsList
    {
        [JsonProperty("staffsList")]
        public List<AdminStaffListDetails> StaffsList { get; set; }

        [JsonProperty("itemsCount")]
        public int ItemsCount { get; set; }

        [JsonProperty("countryOptions")]
        public List<DropdownOptions> CountryOptions { get; set; }

        [JsonProperty("adminTypeOptions")]
        public List<DropdownOptions> AdminTypeOptions { get; set; }

        [JsonProperty("adminLeaderOptions")]
        public List<DropdownOptions> AdminLeaderOptions { get; set; }

        [JsonProperty("taskOptions")]
        public List<DropdownOptions> TaskOptions { get; set; }

    }

    public class AdminStaffListDetails
    {
        [JsonProperty("adminStaffId")]
        public int AdminStaffId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("adminType")]
        public string AdminType { get; set; }

        [JsonProperty("leader")]
        public string Leader { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }
    }
}