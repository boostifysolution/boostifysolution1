using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserSubmitTaskDetails
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("newStatus")]
        public int NewStatus { get; set; }
    }
}