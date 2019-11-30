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
	public class IngredientController : Controller
	{
		private IRecipeRepository repository;
		public int PageSize = 10;
		public IngredientController(IRecipeRepository repo)
		{
			repository = repo;
		}

		// GET: /Ingredient/
		[AllowAnonymous]
		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		// GET: /Ingredient/List
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
			ViewData["UpdatedSortParm"] = sortOrder == "date" ? "date_desc" : "date";

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}
			ViewData["CurrentFilter"] = searchString;

			var list = from s in repository.Ingredients
					   select s;

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
				case "date":
					list = list.OrderBy(s => s.Updated);
					break;
				case "date_desc":
					list = list.OrderByDescending(s => s.Updated);
					break;
				default:
					list = list.OrderBy(s => s.ID);
					break;
			}

			ViewBag.Message = "Yummy Mummy's Kitchen Recipe Ingredient List";
			return View(await PaginatedList<Ingredient>.CreateAsync(list, pageNumber ?? 1, PageSize));
		}


		//GET create /Ingredient/Add
		[HttpGet]
		public ViewResult Add()
		{
			ViewBag.Message = "Add a new Ingredient";
			return View(new Ingredient());
		}

		//POST /Ingredient/Add
		[HttpPost]
		public ActionResult Add(Ingredient formdata)
		{
			if (ModelState.IsValid)
			{
				formdata.ID = 0;
				repository.SaveIngredient(formdata);
				TempData["message"] = "You have added a new Ingredient [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else //if there is something wrong with the data values
			{				
				return View(formdata);
			}
		}

		//GET edit /Ingredient/Update/{ID}
		//[HttpGet]
		public ViewResult Update(int ID)
		{
			Ingredient found = repository.GetIngredient(ID);
			ViewBag.Message = "Edit Ingredient";
			return View(found);
		}

		//POST edit /Ingredient/Update/{ID}
		[HttpPost]
		public ActionResult Update(Ingredient formdata)
		{
			if (ModelState.IsValid) //input is Valided?
			{
				repository.SaveIngredient(formdata);
				TempData["message"] = "You have Updated the Ingredient's name to [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else //if there is something wrong with the data values
			{				
				return View(formdata);
			}
		}

		//GET delete /Ingredient/Delete/{ID} confirm page
		[HttpGet]
		public ViewResult Delete(int ID)
		{
			Ingredient found = repository.GetIngredient(ID);
			ViewBag.Message = "Are you sure want to delete the ingredient [" + found.Name + "]  ?";
			return View(found);
		}

		//POST delete /Ingredient/Delete/{ID}
		[HttpPost, ActionName("Delete")] //action is delete
		public ActionResult DeleteConfirmed(int ID) // same argument with delete above, so change to DeleteConfirmed
		{
			Ingredient found = repository.DeleteIngredient(ID);
			if (found != null)
			{
				TempData["message"] = "Ingredient [" + found.Name + "] was deleted successfully.";
			}
			else
			{
				TempData["message"] = "Failed to delete the ingredient [" + ID + "] as it is not existed. Ingredient id:";
			}
			return RedirectToAction("List");
		}

		// Get summary /Ingredient/Details/{ID}
		[HttpGet]  // Only get, no post
		[AllowAnonymous]
		public ViewResult Details(int ID)
		{
			Ingredient found = repository.GetIngredient(ID);
			if (found == null)
			{
				ViewBag.errorMessage = string.Format("Ingredient {0} not found", ID);
			}
			ViewBag.Message = "Ingredient Summary";
			return View(found);
		}
	}
}
