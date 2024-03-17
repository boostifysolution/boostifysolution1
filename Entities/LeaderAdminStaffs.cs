using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BoostifySolution.Entities;

namespace boostifysolution1.Entities
{
    public class LeaderAdminStaffs
    {
        [Required]
        public int AdminStaffId { get; set; }

        [ForeignKey("AdminStaffId")]
        public virtual AdminStaffs AdminStaff { get; set; }

        [Required]
        public int AssociatedAdminStaffId { get; set; }
    }
}