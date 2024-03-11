using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoostifySolution.Models.Global;

namespace BoostifySolution.Models.Admin
{
    public class UserTaskDetails
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
        public List<string> ProductImagesURL { get; set; }

        [JsonProperty("productPrice")]
        public string ProductPrice { get; set; }

        [JsonProperty("productRating")]
        public double ProductRating { get; set; }

         [JsonProperty("productRatingWidth")]
        public double ProductRatingWidth { get; set; }

        [JsonProperty("productRatingCount")]
        public int ProductRatingCount { get; set; }

        [JsonProperty("productSoldCount")]
        public int ProductSoldCount { get; set; }

        [JsonProperty("quantityRemaining")]
        public int QuantityRemaining { get; set; }

        [JsonProperty("shippingFee")]
        public string ShippingFee { get; set; }

        [JsonProperty("vouchers")]
        public List<string> Vouchers { get; set; }

        [JsonProperty("storeName")]
        public string StoreName { get; set; }

        [JsonProperty("storeThumbnailURL")]
        public string StoreThumbnailURL { get; set; }

        [JsonProperty("beforeDiscountPrice")]
        public string BeforeDiscountPrice { get; set; }

        [JsonProperty("DiscountPercentage")]
        public string DiscountPercentage { get; set; }

        [JsonProperty("productReviewsList")]
        public List<ProductReviewListDetails> ProductReviewsList { get; set; }
    }

    public class ProductReviewListDetails
    {
        [JsonProperty("reviewerName")]
        public string ReviewerName { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

         [JsonProperty("variation")]
        public string Variation { get; set; }
    }
}