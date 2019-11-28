using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YummyMummy.Models;
using YummyMummy.Models.ViewModels;


namespace YummyMummy.Controllers
{
	[Authorize]
	public class RecipeController : Controller
    {
		private IRecipeRepository repository;
		public int PageSize = 100;		
		public RecipeController(IRecipeRepository repo)
		{
			repository = repo;
		}

		[AllowAnonymous]
		public ViewResult List(int recipePage = 1) {
			var list = repository.Recipes;
			if (User.Identity.IsAuthenticated && User.IsInRole("User")) {
				list = list.Where(r => r.UserName == User.Identity.Name);
			}
			list = list.OrderBy(r => r.ID)
					.Skip((recipePage - 1) * PageSize)
					.Take(PageSize);


			foreach (var p in list) {
				p.Category = repository.GetCategory(p.CategoryID);
			}
			return View(list);
			//PagingInfo = new PagingInfo
			//	{
			//		CurrentPage = recipePage,
			//		ItemsPerPage = PageSize,
			//		TotalItems = repository.Recipes.Count()
			//	});
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
				TempData["message"] = "!!!You are not the owner, you can't Delete the Recipe!";
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