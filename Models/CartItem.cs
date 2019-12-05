using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	public class CartItem : BaseEntity
	{
		public int ID { get; set; }
		public int Amount { get; set; } // default value: 1
		public int SortNumber { get; set; } // the no. in the shopping cart

		public int RecipeID { get; set; }
		public Recipe Recipe { get; set; }

		public string CartID { get; set; }
	}
}
