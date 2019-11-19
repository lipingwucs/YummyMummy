using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YummyMummy.Models.ViewModels
{
	public class RecipesListViewModel
	{
		public IEnumerable<Recipe> Recipes { get; set; }
		public PagingInfo PagingInfo { get; set; }
	}
}
	
