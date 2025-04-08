using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public class TransactionType
    {
		[Key]
		public int TransactionTypeId {  get; set; }

		[Required]
		[MaxLength(100, ErrorMessage = "Transaction name can not be more than 100 characters")]
		[MinLength(1, ErrorMessage = "Transaction name can not be empty")]
		public string TransactionTypeName { get; set; }

		[Required]
		[MaxLength(300, ErrorMessage = "Transaction photo can not be more than 300 characters")]
		public string PathToPhoto {  get; set; }
	}
}
