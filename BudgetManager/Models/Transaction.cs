using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public class Transaction
    {
		[Key]
		public int TransactionId { get; set; }

		[Required]
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public int TransactionTypeId { get; set; }
		[ForeignKey("TransactionTypeId")]
		public TransactionType TransactionType { get; set; }
		
		public decimal TransactionAmount { get; set; }
		public DateTime TransactionsDate { get; set; }
	}
}
