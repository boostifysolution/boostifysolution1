using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
  public class AdminNewStaffRequest
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("loginUsername")]
    public string LoginUsername { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("referralCode")]
    public string ReferralCode { get; set; }

    [JsonProperty("firstTaskId")]
    public int FirstTaskId { get; set; }

    [JsonProperty("adminType")]
    public int? AdminType { get; set; }

    [JsonProperty("adminLeaderStaffId")]
    public int? AdminLeaderStaffId { get; set; }
  }
}