using Microsoft.AspNetCore.Mvc;
using YummyMummy.Models;
using YummyMummy.Models.CartViewModels;
using System.Collections.Generic;

namespace YummyMummy.Components
{
	public class ShoppingCartSummary : ViewComponent
	{
		private readonly Cart _shoppingCart;

		public ShoppingCartSummary(Cart shoppingCart)
		{
			_shoppingCart = shoppingCart;
		}

		public IViewComponentResult Invoke()
		{
			var items = _shoppingCart.GetCartItems();
			_shoppingCart.CartItems = items;

			var model = new CartIndexModel
			{
				Cart = _shoppingCart,
				TotalCost = _shoppingCart.GetCartTotalCost(),
				TotalCookingTime = _shoppingCart.GetCartTotalCookingTime()
				
			};
			return View(model);
		}
	}
}
