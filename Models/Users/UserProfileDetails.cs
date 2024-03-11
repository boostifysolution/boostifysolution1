using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserProfileDetails
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("dateJoined")]
        public string DateJoined { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

         [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("accountAdmin")]
        public string AccountAdmin { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("languageOptions")]
        public List<DropdownOptions> LanguageOptions { get; set; }
    }
}