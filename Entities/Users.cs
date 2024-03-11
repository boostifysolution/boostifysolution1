using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public int UserLoginId { get; set; }

        [ForeignKey("UserLoginId")]
        public virtual UserLogins UserLogin { get; set; }

        public int AdminStaffId { get; set; }

        [ForeignKey("AdminStaffId")]
        public virtual AdminStaffs AdminStaff { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public int Currency { get; set; }

        [Required]
        public int Language { get; set; }

        [Required]
        public int Country { get; set; }

        [Required]
        public int AccountStatus { get; set; }

        [Required]
        public decimal WalletAmount { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public bool ShowPopupMessage { get; set; }

        public string? PopupMessageTitle { get; set; }

        public string? PopupMessage { get; set; }
    }
}