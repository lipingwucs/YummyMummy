using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	public class MenuItem : BaseEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] //this line
		public int ID { get; set; }
		public int Amount { get; set; }
		public int SortNumber { get; set; } // the no. in the menu

		public int RecipeID { get; set; }
		public Recipe Recipe { get; set; }

		public int MenuID { get; set; }
		public Menu Menu { get; set; }

	}
}
