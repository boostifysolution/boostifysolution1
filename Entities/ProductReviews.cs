using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class ProductReviews
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductReviewId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReviewerName { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}