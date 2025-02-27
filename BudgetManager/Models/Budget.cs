using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public class Budget
    {
		[Key]
		public int BudgetId { get; set; }

		[Required]
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public int TransactionTypeId { get; set; }
		[ForeignKey("TransactionTypeId")]
		public TransactionType TransactionType { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public decimal BudgetAmount { get; set; }
		public decimal SpentAmount { get; set; }
	}
}
