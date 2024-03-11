using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class WalletTransactions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int WalletTransactionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int WalletTransactionType { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [MaxLength(30)]
        public string? BankName { get; set; }

        [MaxLength(20)]
        public string? BankAccountNumber { get; set; }

        [MaxLength(50)]
        public string? AccountHolderName { get; set; }
    }
}