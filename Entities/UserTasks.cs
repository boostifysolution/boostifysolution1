using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoostifySolution.Entities
{
    public class UserTasks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserTaskId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual Tasks Task { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public decimal CommissionPercentage { get; set; }
        public decimal? CommissionAmount { get; set; }

        public int? QuantityPurchased { get; set; }

        public DateTime? CompletedDateTime { get; set; }
    }
}