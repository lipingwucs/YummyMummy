using Microsoft.EntityFrameworkCore;
using YummyMummy.Data;
using YummyMummy.Data.Enums;
using YummyMummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YummyMummy.Services
{
	public class MenuService : IMenu
	{
		private readonly RecipeDbContext _context;
		private readonly Cart _shoppingCart;

		public MenuService(RecipeDbContext context, Cart shoppingCart)
		{
			_context = context;
			_shoppingCart = shoppingCart;
		}

		public void CreateMenu(Menu menu)
		{
			menu.MenuCreated = DateTime.Now;
			menu.TotalCost = _shoppingCart.GetCartTotalCost();
			menu.TotalCookingTime = _shoppingCart.GetCartTotalCookingTime();
			_context.Add(menu);

			var cartItems = _shoppingCart.GetCartItems();
			var menuItems = new List<MenuItem>(cartItems.Count());

			foreach (var item in cartItems)
			{
				menuItems.Add(
					new MenuItem
					{
						MenuID = menu.ID,
						RecipeID = item.Recipe.ID,
						Amount = item.Amount, // Math.Min(item.Amount, item.Recipe.InStock),
						Recipe = item.Recipe
					});
			}
			_context.MenuItems.AddRange(menuItems);
			_context.SaveChanges();
			// clear shopping cart after saved it to mymenu
			_shoppingCart.ClearCart();
		}

		public Menu GetById(int ID)
		{
			return GetAll()
				.FirstOrDefault(menu => menu.ID == ID);
		}

		public IEnumerable<Menu> GetByUserId(string userId)
		{
			return GetAll()
				.Where(menu => menu.UserID == userId);
		}

		public IEnumerable<Menu> GetFilteredMenus(
			string userId,
			OrderBy orderBy = OrderBy.None,
			int offset = 0, int limit = 10,
			double? minimalPrice = null,
			double? maximalPrice = null,
			DateTime? minDate = null,
			DateTime? maxDate = null,
			string description = null)
		{
			var menus = string.IsNullOrEmpty(userId) ? GetAll() : GetByUserId(userId);

			if (orderBy != OrderBy.None)
			{
				SetMenuBy(menus, orderBy);
			}

			if (minimalPrice.HasValue)
			{
				menus = menus.Where(menu => menu.TotalCost > minimalPrice);
			}

			if (maximalPrice.HasValue)
			{
				menus = menus.Where(menu => menu.TotalCost > maximalPrice);
			}

			if (minDate.HasValue)
			{
				menus = menus.Where(menu => menu.MenuCreated > minDate.Value);
			}

			if (maxDate.HasValue)
			{
				menus = menus.Where(menu => menu.MenuCreated < maxDate.Value);
			}

			if (!string.IsNullOrEmpty(description))
			{
				menus = menus.Where(menu => menu.Description.Contains(description));
			}

			return menus.Skip(offset).Take(limit);
		}

		private void SetMenuBy(IEnumerable<Menu> menus, OrderBy menuBy)
		{
			switch (menuBy)
			{
				case OrderBy.DateDesc:
					menus = menus.OrderByDescending(menu => menu.MenuCreated);
					break;
				case OrderBy.DateAsc:
					menus = menus.OrderBy(menu => menu.MenuCreated);
					break;
				case OrderBy.PriceAsc:
					menus = menus.OrderBy(menu => menu.TotalCost);
					break;
				case OrderBy.PriceDesc:
					menus = menus.OrderByDescending(menu => menu.TotalCost);
					break;
			}
		}

		

		public IEnumerable<Menu> GetAll()
		{
			return _context.Menus
				.Include(menu => menu.MenuItems)
				.ThenInclude(line => line.Recipe)
				.AsNoTracking();
		}
	}
}