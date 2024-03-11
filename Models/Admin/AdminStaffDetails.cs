using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
  public class AdminStaffDetails
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("loginUsername")]
    public string LoginUsername { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("adminType")]
    public int AdminType { get; set; }

    [JsonProperty("adminTypeOptions")]
    public List<DropdownOptions> AdminTypeOptions { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("accounts")]
    public int Accounts { get; set; }

    [JsonProperty("country")]
    public int? Country { get; set; }

    [JsonProperty("dateAdded")]
    public string DateAdded { get; set; }

    [JsonProperty("usersList")]
    public List<AdminStaffUserListDetails> UsersList { get; set; }
  }

  public class AdminStaffUserListDetails
  {
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("walletAmount")]
    public string WalletAmount { get; set; }

    [JsonProperty("dateAdded")]
    public string DateAdded { get; set; }
  }
}