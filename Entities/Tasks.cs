using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class Tasks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        public string TaskTitle { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductMainImageURL { get; set; }

        public string ProductImagesURL { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public double ProductRating { get; set; }

        public string StoreName { get; set; }

        public string StoreThumbnailURL { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int Language { get; set; }

        [Required]
        public int Platform { get; set; }

        [Required]
        public int TaskCategory { get; set; }

        [Required]
        public int Country { get; set; }


        [Required]
        public string ProductLink { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}