using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace boostifysolution1.Models.Users
{
    public class UserSignUpRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("country")]
        public int Country { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("referralCode")]
        public string ReferralCode { get; set; }
    }
}