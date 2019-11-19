using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YummyMummy.Models;

namespace YummyMummy.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
    {

		private IRecipeRepository repository;

		//constructor
		public CategoryController(IRecipeRepository repo)
		{
			repository = repo;
		
		}

		// GET: /Category/
		[AllowAnonymous]
		public ActionResult Index()
        {
			return RedirectToAction("List"); //redirect to list
		}

		// GET: /Category/List
		[AllowAnonymous]
		public ActionResult List()
		{
			ViewBag.Message = "Yummy Mummy's Kitchen Recipe Category List.";
			return View(repository.Categories);
		}

		//GET create /Category/Add
		public ViewResult Add()
		{
			ViewBag.Message = "Add a new Category";
			return View(new Category());
		}

		//POST /Category/Add
		[HttpPost]
		public ActionResult Add(Category formdata)
		{
			if (ModelState.IsValid)
			{
				formdata.ID = 0;
				repository.SaveCategory(formdata);
				TempData["message"] = "You have added a new Category [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else
			{
				//if there is something wrong with the data values
				return View(formdata);
			}
		}

		//GET edit /Category/Update/{ID}
		public ViewResult Update(int ID)
		{
			Category found = repository.GetCategory(ID);
			ViewBag.Message = "Edit Category";
			return View(found);
		}

		//POST edit /Category/Update/{ID}
		[HttpPost]
		public ActionResult Update(Category formdata)
		{
			if (ModelState.IsValid)
			{
				repository.SaveCategory(formdata);
				TempData["message"] = "You have Updated the Category's name to [" + formdata.Name + "] Successfully! ";
				return RedirectToAction("List");
			}
			else
			{
				//if there is something wrong with the data values
				return View(formdata);
			}
		}

		//GET delete /Category/Delete/{ID} confirm page
		[HttpGet]
		public ViewResult Delete(int ID)
		{
			Category found = repository.GetCategory(ID);
			ViewBag.Message = "Are you sure want to delete the category [" + found.Name + "]  ?";
			return View(found);
		}

		//POST delete /Category/Delete/{ID}
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int ID)
		{
			Category found = repository.DeleteCategory(ID);
			if (found != null)
			{
				TempData["message"] = "Category [" + found.Name + "] was deleted successfully.";
			}
			else
			{
				TempData["message"] = "Failed to delete Category [" + ID + "] as it is not existed. Category id:";
			}
			return RedirectToAction("List");
		}

		// Get summary /Category/Details/{ID}
		[AllowAnonymous]
		public ViewResult Details(int ID)
		{
			Category found = repository.GetCategory(ID);
			if (found == null)
			{
				ViewBag.errorMessage = string.Format("Category {0} not found", ID);
			}
			ViewBag.Message = "Category Summary";
			return View(found);
		}
			
    }
}
