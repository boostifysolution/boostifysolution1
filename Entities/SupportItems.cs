using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class SupportItems
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SupportItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int ProductCurrency { get; set; }

        [Required]
        public int ProductLanguage { get; set; }

        [Required]
        [MaxLength(300)]
        public string ProductImageURL { get; set; }

        [Required]
        public int ProductRating { get; set; }

        [Required]
        public int ProductRatingCount { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}