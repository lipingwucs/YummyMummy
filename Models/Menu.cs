using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	public class Menu : BaseEntity
	{
		public int ID { get; set; }

		public string Name { get; set; }
		public DateTime MenuCreated { get; set; }
		[Range(1, 100000)]
		public double TotalCost { get; set; }
		[Range(1, 10000)]
		public int TotalCookingTime { get; set; }
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public string UserID { get; set; } // the registered login name
		public AppUser User { get; set; }
		public IEnumerable<MenuItem> MenuItems { get; set; }
	}
}
