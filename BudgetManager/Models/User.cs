using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public class User
    {
		[Key]
		public int UserId { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[MaxLength(20, ErrorMessage = "Name must can not be more than 20 characters or less than 2")]
		[MinLength(2)]
		public string Name { get; set; }

		[Required]
		[MaxLength(50, ErrorMessage = "Name must can not be more than 50 characters or less than 2")]
		[MinLength(2)]
		public string LastName { get; set; }

		public decimal Balance { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
    }
}
