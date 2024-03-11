using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminTasksList
    {
        [JsonProperty("tasksList")]
        public List<AdminTaskListDetails> TasksList { get; set; }

        [JsonProperty("itemsCount")]
        public int ItemsCount { get; set; }

        [JsonProperty("taskCategoryOptions")]
        public List<DropdownOptions> TaskCategoryOptions { get; set; }

        [JsonProperty("platformOptions")]
        public List<DropdownOptions> PlatformOptions { get; set; }

        [JsonProperty("countryOptions")]
        public List<DropdownOptions> CountryOptions { get; set; }

        [JsonProperty("languageOptions")]
        public List<DropdownOptions> LanguageOptions { get; set; }

        [JsonProperty("taskStatusOptions")]
        public List<DropdownOptions> TaskStatusOptions { get; set; }
    }

    public class AdminTaskListDetails
    {
        [JsonProperty("taskId")]
        public int TaskId { get; set; }

        [JsonProperty("taskTitle")]
        public string TaskTitle { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("productPrice")]
        public string ProductPrice { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }
    }
}