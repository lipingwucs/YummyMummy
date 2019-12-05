using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YummyMummy.Models;
using YummyMummy.Models.CartViewModels;

namespace YummyMummy.Controllers
{
	[Authorize]
	public class CartController : Controller
	{

		private IRecipeRepository repository;
		private readonly Cart _shoppingCart;

		//constructor
		public CartController(IRecipeRepository repo, Cart shoppingCart)
		{
			repository = repo;
			_shoppingCart = shoppingCart;
		}

		public IActionResult Index(bool isValidAmount = true, string returnUrl = "/")
		{
			_shoppingCart.GetCartItems();

			var model = new CartIndexModel
			{
				Cart = _shoppingCart,
				TotalCost = _shoppingCart.GetCartTotalCost(),
				TotalCookingTime = _shoppingCart.GetCartTotalCookingTime(),
				IngredientsList = _shoppingCart.GetIngredientsListWithAmount(),
				ReturnUrl = returnUrl
			};

			if (!isValidAmount)
			{
				ViewBag.InvalidAmountText = "*There were not enough items in stock to add*";
			}

			return View("Index", model);
		}

		public IActionResult AddToCart(int recipeId, int? amount = 1, string returnUrl = null)
		{
			var recipe = repository.GetRecipe(recipeId);
			returnUrl = returnUrl.Replace("%2F", "/");
			bool isValidAmount = false;
			if (recipe != null)
			{
				isValidAmount = _shoppingCart.AddToCart(recipe, amount.Value);
			}

			return Index(isValidAmount, returnUrl);
		}

		public IActionResult RemoveFromCart(int recipeId)
		{
			var recipe = repository.GetRecipe(recipeId);
			if (recipe != null)
			{
				_shoppingCart.RemoveFromCart(recipe);
			}
			return RedirectToAction("Index");
		}

		public IActionResult Back(string returnUrl = "/")
		{
			return Redirect(returnUrl);
		}
	}
}