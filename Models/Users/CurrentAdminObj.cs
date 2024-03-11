using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoostifySolution.Models.Users
{
    public class CurrentAdminObj
    {
        [JsonProperty("adminStaffId")]
        public int AdminStaffId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("adminStaffType")]
        public int AdminStaffType { get; set; }

        [JsonProperty("requiredPasswordChange")]
        public bool RequirePasswordChange { get; set; }

    }
}