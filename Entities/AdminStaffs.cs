using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class AdminStaffs
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AdminStaffId { get; set; }

        public int UserLoginId { get; set; }

        [ForeignKey("UserLoginId")]
        public virtual UserLogins UserLogin { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public int AdminStaffType { get; set; }

        public int? Country { get; set; }

        [Required]
        public bool RequirePasswordChange { get; set; }
    }
}