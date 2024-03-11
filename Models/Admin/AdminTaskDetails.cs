using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class AdminTaskDetails
    {
        [JsonProperty("taskTitle")]
        public string TaskTitle { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("productMainImageURL")]
        public string ProductMainImageURL { get; set; }

        [JsonProperty("productImagesURL")]
        public string ProductImagesURL { get; set; }

        [JsonProperty("productPrice")]
        public decimal ProductPrice { get; set; }

        [JsonProperty("productRating")]
        public double ProductRating { get; set; }

        [JsonProperty("storeName")]
        public string StoreName { get; set; }

        [JsonProperty("storeThumbnailURL")]
        public string StoreThumbnailURL { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

         [JsonProperty("statusId")]
        public int StatusId { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("platform")]
        public int Platform { get; set; }

        [JsonProperty("taskCategory")]
        public int TaskCategory { get; set; }

        [JsonProperty("country")]
        public int Country { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }
    }

}