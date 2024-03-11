using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminAddPopupMessageDetails
    {
        [JsonProperty("showPopupMessage")]
        public bool ShowPopupMessage { get; set; }

        [JsonProperty("popupMessageTitle")]
        public string? PopupMessageTitle { get; set; }

        [JsonProperty("popupMessage")]
        public string? PopupMessage { get; set; }
    }
}