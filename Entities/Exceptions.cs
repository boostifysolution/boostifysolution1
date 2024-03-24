using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class Exceptions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ExceptionsId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? Data { get; set; }

        [Required]
        public string ExceptionTitle { get; set; }

        [Required]
        public string ExceptionType { get; set; }

        [Required]
        public string ExceptionMessage { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}