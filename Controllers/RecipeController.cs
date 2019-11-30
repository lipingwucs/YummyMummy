using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YummyMummy.Models;
using YummyMummy.Models.ViewModels;
using YummyMummy.Infrastructure;


namespace YummyMummy.Controllers
{
	[Authorize]
	public class RecipeController : Controller
    {
		private IRecipeRepository repository;
		public int PageSize = 4;
		public RecipeController(IRecipeRepository repo)
		{
			repository = repo;
		}

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
			ViewData["CategorySortParm"] = sortOrder == "category" ? "category_desc" : "category";
			ViewData["CookingtimeSortParm"] = sortOrder == "cookingtime" ? "cookingtime_desc" : "cookingtime";
			ViewData["CostSortParm"] = sortOrder == "cost" ? "cost_desc" : "cost";
			ViewData["AuthorSortParm"] = sortOrder == "username" ? "username_desc" : "username";
			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}
			ViewData["CurrentFilter"] = searchString;

			//var list = repository.Recipes;
			var list = from s in repository.Recipes
					   select s;
			// only list the recipes of the login users unless it is admin
			if (User.Identity.IsAuthenticated && !User.IsInRole("Admin")) {
				list = list.Where(r => r.UserName == User.Identity.Name);
			}
			if (!String.IsNullOrEmpty(searchString))
			{
				list = list.Where(s => s.Name.Contains(searchString)
									   || s.UserName.Contains(searchString));
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
				case "category":
					list = list.OrderBy(s => s.CategoryID);
					break;
				case "category_desc":
					list = list.OrderByDescending(s => s.CategoryID);
					break;
				case "cookingtime":
					list = list.OrderBy(s => s.CookingTime);
					break;
				case "cookingtime_desc":
					list = list.OrderByDescending(s => s.CookingTime);
					break;
				case "cost":
					list = list.OrderBy(s => s.Cost);
					break;
				case "cost_desc":
					list = list.OrderByDescending(s => s.Cost);
					break;
				case "username":
					list = list.OrderBy(s => s.UserName);
					break;
				case "username_desc":
					list = list.OrderByDescending(s => s.UserName);
					break;
				default:
					list = list.OrderBy(s => s.ID);
					break;
			}

			var data = await PaginatedList<Recipe>.CreateAsync(list, pageNumber ?? 1, PageSize);
			foreach (var p in data)
			{
				p.Category = repository.GetCategory(p.CategoryID);
			}
			return View(data);
		}

		// GET: Recipe
		[AllowAnonymous]
		public ActionResult Index()
        {
			return RedirectToAction("List");
			//return View();
        }
		public ViewResult Add()
		{
			ViewBag.Message = "Add a new recipe to Yummy Mummy's Kitchen ";
			this.PopulateCategoryDropDownList(0);
			Recipe formdata = new Recipe();
			formdata.UserName = User.Identity.Name;
			return View(formdata);
		}

		//POST /Recipe/Add
		[HttpPost]
		public ActionResult Add(Recipe formdata)
		{
			if (ModelState.IsValid)
			{
				formdata.ID = 0;
				formdata.UserName = User.Identity.Name;
				repository.SaveRecipe(formdata);
				TempData["message"] = "You have added a new Recipe [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else
			{
				//if there is something wrong with the data values
				this.PopulateCategoryDropDownList(formdata.CategoryID);
				return View(formdata);
			}
		}

		private void PopulateCategoryDropDownList(object selectedCategory = null)
		{
			ViewBag.CategoryID = new SelectList(this.repository.Categories, "ID", "Name", selectedCategory);
		}

		//GET edit /Recipe/Update/{ID}
		public ActionResult Update(int ID)
		{
			Recipe found = repository.GetRecipe(ID);
			if (!User.IsInRole("Admin")  && User.Identity.Name!=found.UserName)
			{
				TempData["message"] = "!!!You are not the owner, you can't Update the Recipe!";
				return RedirectToAction(nameof(Details), new { id = found.ID });
			}
			ViewBag.Message = "Edit Recipe";
			this.PopulateCategoryDropDownList(found.CategoryID);
			return View(found);
		}

		//POST edit /Recipe/Update/{ID}
		[HttpPost]
		public ActionResult Update(Recipe formdata)
		{
			if (ModelState.IsValid)
			{
				if (!User.IsInRole("Admin") && User.Identity.Name != formdata.UserName)
				{
					TempData["message"] = "!!!You are not the owner, you can't Update the Recipe!";
					return RedirectToAction(nameof(Details), new { id = formdata.ID });
				}
				repository.SaveRecipe(formdata);
				TempData["message"] = "You have Updated the Recipe [" + formdata.Name + "] information Successfully! ";
				return RedirectToAction("List");
			}
			else
			{
				//if there is something wrong with the data values
				this.PopulateCategoryDropDownList(formdata.CategoryID);
				return View(formdata);
			}
		}

		//GET delete /Recipe/Delete/{ID} confirm page
		[HttpGet]
		public ActionResult Delete(int ID)
		{
			Recipe found = repository.GetRecipe(ID);
			if (!User.IsInRole("Admin") && User.Identity.Name != found.UserName)
			{
				TempData["message"] = "!!!You are not the author, you can't Delete the Recipe!";
				return RedirectToAction(nameof(Details), new { id = found.ID });
			}
			ViewBag.Message = "Are you sure want to delete the recipe [" + found.Name + "]  ?";
			return View(found);
		}

		//POST delete /Recipe/Delete/{ID}
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int ID)
		{
			Recipe found = repository.DeleteRecipe(ID);
			if (found != null)
			{
				TempData["message"] = "Recipe [" + found.Name + "] was deleted successfully.";
			}
			else
			{
				TempData["message"] = "Failed to delete the recipe [" + ID + "] as it is not existed. Recipe id:";
			}
			return RedirectToAction("List");
		}

		// Get summary /Recipe/Details/{ID}
		[AllowAnonymous]
		public ViewResult Details(int ID)
		{
			Recipe found = repository.GetRecipe(ID);
			if (found == null)
			{
				ViewBag.errorMessage = string.Format("Recipe {0} not found", ID);
			}
			ViewBag.Message = "Recipe Summary";
			return View(found);
		}

		//Get: Add Ingredient to Recipe page /Recipe/AddIngredient/{RecipeID}
		public ViewResult AddIngredient(int ID) {
			Recipe r = repository.GetRecipe(ID);
			ViewBag.Message = "Add a Ingredient to the recipe ["+r.Name+"] ";
			RecipeIngredient found  = new RecipeIngredient();
			found.RecipeID = ID;
			found.Recipe = r;
			this.PopulateIngredientsDropDownList(found.IngredientID);
			return View(found);
		}

		//POST: Add Ingredient to Recipe page /Recipe/AddIngredient/{RecipeID}
		[HttpPost]
		public IActionResult AddIngredient(RecipeIngredient formdata)
		{
			if (ModelState.IsValid)
			{
				var exists = repository.RecipeIngredients.Where(p => (p.RecipeID == formdata.RecipeID && p.IngredientID == formdata.IngredientID));
				if (!exists.Any())
				{
					formdata.ID = 0;
					repository.SaveRecipeIngredient(formdata);
					RecipeIngredient found = repository.GetRecipeIngredient(formdata.ID);
					TempData["message"] = "Ingredient [" + found.Ingredient.Name + "] has been added to recipe [" + found.Recipe.Name + "].";
					return RedirectToAction("Details", new { id = found.RecipeID });
					//ViewBag.Message = "Ingredient [" + found.Ingredient.Name + "] has been added to recipe ["+ found.Recipe.Name+"].";
					// return View("actionResult");
				}
				else {
					TempData["message"] = "The Ingredient has already been added to this Recipe.";
				}
			}
			
			//if there is something wrong with the data values
			formdata.Recipe = repository.GetRecipe(formdata.RecipeID);
			this.PopulateIngredientsDropDownList(formdata.IngredientID);
			return View(formdata);			
		}

		private void PopulateIngredientsDropDownList(object selectedIngredient = null)
		{
			ViewBag.IngredientID = new SelectList(this.repository.Ingredients, "ID", "Name", selectedIngredient);
		}

		//Get: Update Ingredient unit and amount to Recipe page /Recipe/UpdateIngredient/{RecipeIngredientID}
		public ViewResult UpdateIngredient(int ID)
		{
			RecipeIngredient found = repository.GetRecipeIngredient(ID);
			ViewBag.Message = "Edit Ingredient [" + found.Ingredient.Name + "] amount to recipe [" + found.Recipe.Name + "].";
			this.PopulateIngredientsDropDownList(found.IngredientID);
			return View(found);
		}

		//POST: Update Ingredient unit and amount to Recipe page /Recipe/UpdateIngredient/{RecipeIngredientID}
		[HttpPost]
		public IActionResult UpdateIngredient(RecipeIngredient formdata)
		{
			if (ModelState.IsValid)
			{
				var exists = repository.RecipeIngredients
					.Where(p => (p.RecipeID == formdata.RecipeID 
								 && p.IngredientID == formdata.IngredientID
								 && p.ID != formdata.ID));
				if (!exists.Any())
				{
					repository.SaveRecipeIngredient(formdata);
					RecipeIngredient found = repository.GetRecipeIngredient(formdata.ID);
					TempData["message"] = "Ingredient [" + found.Ingredient.Name + "] amount has been updated to recipe [" + found.Recipe.Name + "].";
					return RedirectToAction(nameof(Details), new { id = found.RecipeID });
					//ViewBag.Message = "Ingredient [" + found.Ingredient.Name + "] has been added to recipe ["+ found.Recipe.Name+"].";
					// return View("actionResult");
				}
				else
				{
					TempData["message"] = "The Ingredient has already been added to this Recipe.";
				}
			}

			//if there is something wrong with the data values
			formdata.Recipe = repository.GetRecipe(formdata.RecipeID);
			this.PopulateIngredientsDropDownList(formdata.IngredientID);
			return View(formdata); 			
		}

		//Get: Delete Ingredient from Recipe page /Recipe/DeleteIngredient/{RecipeIngredientID}
		[HttpGet]
		public ViewResult DeleteIngredient(int ID)
		{
			RecipeIngredient found = repository.GetRecipeIngredient(ID);
			ViewBag.Message = "Are you sure want to remove the ingredient [" + found.Ingredient.Name + "] from recipe [" + found.Recipe.Name + "] ?";
			return View(found);
				
		}

		//POST: Delete Ingredient from Recipe page /Recipe/DeleteIngredient/{RecipeIngredientID}
		[HttpPost, ActionName("DeleteIngredient")]
		public IActionResult DeleteIngredientConfirmed(int ID)
		{
			RecipeIngredient found = repository.DeleteRecipeIngredient(ID);
			TempData["message"] = "Ingredient [" + found.Ingredient.Name + "] has been removed from recipe [" + found.Recipe.Name + "].";
			return RedirectToAction(nameof(Details), new { id = found.RecipeID });
			// ViewBag.Message = "Ingredient [" + found.Ingredient.Name + "] has been removed from recipe [" + found.Recipe.Name + "].";
			// return View("actionResult");
		}

		//Get: Add Review to Recipe page /Recipe/AddReview/{RecipeID}
		public ViewResult AddReview(int ID)
		{
			Recipe r = repository.GetRecipe(ID);
			ViewBag.Message = "Add a Review to the recipe [" + r.Name + "]";
			RecipeReview found = new RecipeReview();
			found.RecipeID = ID;
			found.Recipe = r;
			return View(found);
		}

		//POST: Add Review to Recipe page /Recipe/AddReview/{RecipeID}
		[HttpPost]
		public IActionResult AddReview(RecipeReview formdata)
		{
			if (ModelState.IsValid)
			{
				formdata.ID = 0;
				repository.SaveRecipeReview(formdata);
				RecipeReview found = repository.GetRecipeReview(formdata.ID);
				TempData["message"] = "A Review from " + formdata.Email + " has been added to recipe [" + found.Recipe.Name + "].";
				return RedirectToAction("Details", new { id = found.RecipeID });
			}
			else
			{
				//if there is something wrong with the data values
				formdata.Recipe = repository.GetRecipe(formdata.RecipeID);
				return View(formdata);
			}
		}
		
		//Get: Update Review unit and amount to Recipe page /Recipe/UpdateReview/{RecipeReviewID}
		public ViewResult UpdateReview(int ID)
		{
			RecipeReview found = repository.GetRecipeReview(ID);
			ViewBag.Message = "Edit Review of [" + found.Email + "] on recipe [" + found.Recipe.Name + "]";
			return View(found);
		}

		//POST: Update Review unit and amount to Recipe page /Recipe/UpdateReview/{RecipeReviewID}
		[HttpPost]
		public IActionResult UpdateReview(RecipeReview formdata)
		{
			if (ModelState.IsValid)
			{
				repository.SaveRecipeReview(formdata);
				RecipeReview found = repository.GetRecipeReview(formdata.ID);
				TempData["message"] = "A Review of [" + found.Email + "] has been updated to recipe [" + found.Recipe.Name + "] successfully.";
				return RedirectToAction(nameof(Details), new { id = found.RecipeID });
			}
			else
			{
				//if there is something wrong with the data values
				formdata.Recipe = repository.GetRecipe(formdata.RecipeID);
				return View(formdata);
			}
		}

		//Get: Delete Review from Recipe page /Recipe/DeleteReview/{RecipeReviewID}
		[HttpGet]
		public ViewResult DeleteReview(int ID)
		{
			RecipeReview found = repository.GetRecipeReview(ID);
			ViewBag.Message = "Are you sure want to remove the review of [" + found.Email + "] from recipe [" + found.Recipe.Name + "] ?";
			return View(found);
		}

		//POST: Delete Review from Recipe page /Recipe/DeleteReview/{RecipeReviewID}
		[HttpPost, ActionName("DeleteReview")]
		public IActionResult DeleteReviewConfirmed(int ID)
		{
			RecipeReview found = repository.DeleteRecipeReview(ID);
			TempData["message"] = "The Review of [" + found.Email + "] has been removed from recipe [" + found.Recipe.Name + "].";
			return RedirectToAction(nameof(Details), new { id = found.RecipeID });
		}
	}
}