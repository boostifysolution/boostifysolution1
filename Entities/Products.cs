using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class Products
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductTitle { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int StockCount { get; set; }

        [Required]
        [MaxLength(300)]
        public string ProductPrimaryImage { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ProductSecondaryImagesJson { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ProductDescription { get; set; }

        [Required]
        public int ProductRating { get; set; }

        [Required]
        public int ProductRatingCount { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}