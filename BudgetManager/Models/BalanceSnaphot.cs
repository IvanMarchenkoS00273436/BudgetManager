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
		public int BalanceSnaphotId;

		[Required]
		public int UserId;
		[ForeignKey("UserId")]
		public User User { get; set; }

		public DateTime SnapshotDate;
		public decimal Balance;
	}
}
