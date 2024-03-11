using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminAddTaskDetails
    {
        [JsonProperty("taskId")]
        public int TaskId { get; set; }

        [JsonProperty("commission")]
        public int Commission { get; set; }
    }
}