using System.Linq;
using YummyMummy.Models;

namespace YummyMummy.Models.CartViewModels
{
	public class CartIndexModel
	{
		public Cart Cart { get; set; }
		public double TotalCost { get; set; }
		public double TotalCookingTime { get; set; }
		public string ReturnUrl { get; set; }
		public IQueryable<RecipeIngredient> IngredientsList { get; set; }
	}
}
