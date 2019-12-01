using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YummyMummy.Models;
using YummyMummy.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YummyMummy.Controllers
{
	[Authorize]
	public class MenuController : Controller
	{
		private IRecipeRepository repository;
		public int PageSize = 10;
		public MenuController(IRecipeRepository repo)
		{
			repository = repo;
		}

		// GET: /Menu/
		[AllowAnonymous]
		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		// GET: /Menu/List
		// support filter by search string, sorting, pagination
		[AllowAnonymous]
		public async Task<IActionResult> List(string sortOrder,
			string currentFilter,
			string searchString,
			int? pageNumber)
		{
			ViewData["CurrentSort"] = sortOrder;
			ViewData["IDSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
			ViewData["PlacedSortParm"] = sortOrder == "placed" ? "placed_desc" : "placed";
			ViewData["CookingtimeSortParm"] = sortOrder == "cookingtime" ? "cookingtime_desc" : "cookingtime";
			ViewData["CostSortParm"] = sortOrder == "cost" ? "cost_desc" : "cost";
			

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}
			ViewData["CurrentFilter"] = searchString;

			var list = repository.Menus.Where(r => r.UserID == User.Identity.Name);

			if (!String.IsNullOrEmpty(searchString))
			{
				list = list.Where(s => s.Name.Contains(searchString));
			}
			switch (sortOrder)
			{
				case "id_desc":
					list = list.OrderByDescending(s => s.ID);
					break;
				case "name":
					list = list.OrderBy(s => s.Name);
					break;
				case "name_desc":
					list = list.OrderByDescending(s => s.Name);
					break;
				case "placed":
					list = list.OrderBy(s => s.MenuCreated);
					break;
				case "placed_desc":
					list = list.OrderByDescending(s => s.MenuCreated);
					break;
				case "cookingtime":
					list = list.OrderBy(s => s.TotalCookingTime);
					break;
				case "cookingtime_desc":
					list = list.OrderByDescending(s => s.TotalCookingTime);
					break;
				case "cost":
					list = list.OrderBy(s => s.TotalCost);
					break;
				case "cost_desc":
					list = list.OrderByDescending(s => s.TotalCost);
					break;
				default:
					list = list.OrderBy(s => s.ID);
					break;
			}

			ViewBag.Message = "Yummy Mummy's Kitchen My Menu List";
			return View(await PaginatedList<Menu>.CreateAsync(list, pageNumber ?? 1, PageSize));
		}


		//GET create /Menu/Add
		[HttpGet]
		public ViewResult Add()
		{
			ViewBag.Message = "Add a new Menu";
			return View(new Menu());
		}

		//POST /Menu/Add
		[HttpPost]
		public ActionResult Add(Menu formdata)
		{
			if (ModelState.IsValid)
			{
				formdata.ID = 0;
				repository.SaveMenu(formdata);
				TempData["message"] = "You have added a new Menu [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else //if there is something wrong with the data values
			{				
				return View(formdata);
			}
		}

		//GET edit /Menu/Update/{ID}
		//[HttpGet]
		public ViewResult Update(int ID)
		{
			Menu found = repository.GetMenu(ID);
			ViewBag.Message = "Edit Menu";
			return View(found);
		}

		//POST edit /Menu/Update/{ID}
		[HttpPost]
		public ActionResult Update(Menu formdata)
		{
			if (ModelState.IsValid) //input is Valided?
			{
				repository.SaveMenu(formdata);
				TempData["message"] = "You have Updated the Menu's name to [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else //if there is something wrong with the data values
			{				
				return View(formdata);
			}
		}

		//GET delete /Menu/Delete/{ID} confirm page
		[HttpGet]
		public ViewResult Delete(int ID)
		{
			Menu found = repository.GetMenu(ID);
			ViewBag.Message = "Are you sure want to delete the menu [" + found.Name + "]  ?";
			return View(found);
		}

		//POST delete /Menu/Delete/{ID}
		[HttpPost, ActionName("Delete")] //action is delete
		public ActionResult DeleteConfirmed(int ID) // same argument with delete above, so change to DeleteConfirmed
		{
			Menu found = repository.DeleteMenu(ID);
			if (found != null)
			{
				TempData["message"] = "Menu [" + found.Name + "] was deleted successfully.";
			}
			else
			{
				TempData["message"] = "Failed to delete the menu [" + ID + "] as it is not existed. Menu id:";
			}
			return RedirectToAction("List");
		}

		// Get summary /Menu/Details/{ID}
		[HttpGet]  // Only get, no post
		[AllowAnonymous]
		public ViewResult Details(int ID)
		{
			Menu found = repository.GetMenu(ID);
			if (found == null)
			{
				ViewBag.errorMessage = string.Format("Menu {0} not found", ID);
			}
			ViewBag.Message = "Menu Summary";
			return View(found);
		}
	}
}
