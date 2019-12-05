using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YummyMummy.Data;
using System.Collections;

namespace YummyMummy.Models
{
	public class Cart
	{
		private readonly RecipeDbContext _context;

		public Cart(RecipeDbContext context)
		{
			_context = context;
		}

		public string ID { get; set; }
		public IEnumerable<CartItem> CartItems { get; set; }

		public static Cart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			var context = services.GetService<RecipeDbContext>();
			string cartID = session.GetString("CartID") ?? Guid.NewGuid().ToString();

			session.SetString("CartID", cartID);
			return new Cart(context) { ID = cartID };
		}

		public bool AddToCart(Recipe recipe, int amount)
		{

			var shoppingCartItem = _context.CartItems.SingleOrDefault(
				s => s.Recipe.ID == recipe.ID && s.CartID == ID);
			var isValidAmount = true;
			if (shoppingCartItem == null)
			{
				shoppingCartItem = new CartItem
				{
					CartID = ID,
					RecipeID = recipe.ID,
					SortNumber = 1,
					Amount = amount
				};
				_context.CartItems.Add(shoppingCartItem);
			}
			else
			{
				shoppingCartItem.Amount += amount;
			}

			_context.SaveChanges();
			return isValidAmount;
		}

		public int RemoveFromCart(Recipe recipe)
		{
			if (recipe == null)
			{
				throw new ArgumentNullException(nameof(recipe));
			}

			var shoppingCartItem = _context.CartItems.SingleOrDefault(
				s => s.Recipe.ID == recipe.ID && s.CartID == ID);
			int localAmount = 0;
			if (shoppingCartItem != null)
			{
				_context.CartItems.Remove(shoppingCartItem);
			}

			_context.SaveChanges();
			return localAmount;
		}

		public IEnumerable<CartItem> GetCartItems()
		{
			return CartItems ??
				   (CartItems = _context.CartItems.Where(c => c.CartID == ID)
					   .Include(s => s.Recipe));
		}

		public void ClearCart()
		{
			var cartItems = _context
				.CartItems
				.Where(cart => cart.CartID == ID);

			_context.CartItems.RemoveRange(cartItems);
			_context.SaveChanges();
		}

		public double GetCartTotalCost()
		{
			return _context.CartItems.Where(c => c.CartID == ID)
				.Select(c => c.Recipe.Cost * c.Amount).Sum();
		}

		public int GetCartTotalCookingTime()
		{
			return _context.CartItems.Where(c => c.CartID == ID)
				.Select(c => c.Recipe.CookingTime * c.Amount).Sum();
		}

		public IQueryable<RecipeIngredient> GetIngredientsListWithAmount()
		{
			string mycartID = this.ID;
			var ingredientsList = (from d in _context.RecipeIngredients
								   join f in _context.CartItems
								   on d.RecipeID equals f.RecipeID
								   where (f.CartID == mycartID)
								   select new RecipeIngredient
								   {
									   IngredientID = d.IngredientID,
									   Unit = d.Unit,
									   Amount = d.Amount * f.Amount
								   }).AsNoTracking();

			Dictionary<string, RecipeIngredient> ht = new Dictionary<string, RecipeIngredient>();
			string key = "";
			foreach (var p in ingredientsList)
			{
				key = p.IngredientID + ":" + p.Unit;
				if (ht.ContainsKey(key))
				{
					((RecipeIngredient)ht[key]).Amount += p.Amount;
				} else
				{
					p.Ingredient = _context.Ingredients.Where(x => x.ID == p.IngredientID).FirstOrDefault();
					ht.Add(key, p);
				}
			}
			
			var list = new List<RecipeIngredient>();
			foreach (KeyValuePair<string, RecipeIngredient> kvp in ht) {
				list.Add(kvp.Value);
			}
			return list.AsQueryable();

		}

		public IQueryable<RecipeIngredient> GetIngredientsList()
		{
			string mycartID = this.ID;
			var ingredientsList = (from d in _context.RecipeIngredients
								  join f in _context.CartItems
								  on d.RecipeID equals f.RecipeID
								  where (f.CartID == mycartID)
								  group d by new { d.IngredientID, d.Unit } into g
								  select new RecipeIngredient
								  {
									  IngredientID = g.Key.IngredientID,
									  Unit = g.Key.Unit,
									  Amount = g.Sum(x => x.Amount)
								  }).AsNoTracking();

			foreach (var p in ingredientsList)
			{
				p.Ingredient = _context.Ingredients.Where(x => x.ID==p.IngredientID).FirstOrDefault();
			}
			return ingredientsList;
			
		}

	}
}
