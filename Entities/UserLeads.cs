using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BoostifySolution.Entities;

namespace boostifysolution1.Entities
{
    public class UserLeads
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserLeadId { get; set; }

        [Required]
        public string Name { get; set; }

        public int? AdminStaffId { get; set; }

        [Required]
        public int LeadStatus { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Country { get; set; }
    }
}