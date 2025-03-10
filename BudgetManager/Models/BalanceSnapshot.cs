using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public class BalanceSnaphot
    {
		[Key]
		public int BalanceSnapshotId {  get; set; }

		[Required]
		public int UserId { get; set; }
        [ForeignKey("UserId")]
		public User User { get; set; }

		public DateTime SnapshotDate {  get; set; } 
		public decimal Balance { get; set; }
    }
}
