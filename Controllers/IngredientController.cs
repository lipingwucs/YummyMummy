using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YummyMummy.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YummyMummy.Controllers
{
	[Authorize]
	public class IngredientController : Controller
	{
		private IRecipeRepository repository;
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
		[HttpGet] //View list
		[AllowAnonymous]
		public ActionResult List()
		{
			ViewBag.Message = "Yummy Mummy's Kitchen Recipe Ingredient List.";
			return View(repository.Ingredients);
		}

		// POST: filter by search string /Ingredient/List
		[HttpPost]// search using post
		[AllowAnonymous]
		public ActionResult List(string searchString, bool notUsed)
		{
			ViewBag.Message = "Yummy Mummy's Kitchen Recipe Ingredient List.";
			if (!String.IsNullOrEmpty(searchString))
			{
				var Ingredients = repository.Ingredients.Where(s => s.Name.Contains(searchString));
				return View(Ingredients);
			} else {
				return View(repository.Ingredients);
			}
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
