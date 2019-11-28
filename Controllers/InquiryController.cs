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
	[AllowAnonymous]
	public class InquiryController : Controller
	{
		private IRecipeRepository repository;

		public InquiryController(IRecipeRepository repo)
		{
			repository = repo;
		}

		public ViewResult List() => View(repository.Inquirys);

		// GET: Recipe
		public ActionResult Index()
		{
			return RedirectToAction("List");
			//return View();
		}
		public ActionResult Add()
		{
			ViewBag.Message = "Add a new inquiry to Yummy Mummy's Kitchen ";
			return View(new Inquiry());
		}
		public ActionResult AddNew(Inquiry inquiry)
		{
			repository.SaveInquiry(inquiry);
			ViewBag.Message = "you already added a new inquiry to Yummy Mummy's Kitchen Successfully! ";
			return View("actionResult");
		}

		//Inquiry Delete
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			Inquiry deletedInquiry = repository.DeleteInquiry(id);
			if (deletedInquiry != null)
			{
				ViewBag.Message = deletedInquiry.ID + " was deleted.";
			}
			else
			{
				ViewBag.Message = "Can not found inquiry id:" + id.ToString();
			}

			return View("actionResult");
		}	


		public ActionResult Details(int ID)
		{
			Inquiry found = null;
			foreach (Inquiry r in repository.Inquirys)
			{
				if (r.ID == ID)
				{
					found = r;
				}
			}

			ViewBag.Details = new Dictionary<string, string>();
			if (found == null)
			{
				ViewBag.errorMessage = string.Format("Inquiry {0} not found", ID);
			}
			else
			{
				ViewBag.Details.Add("ID", string.Format("{0}", found.ID));
				ViewBag.Details.Add("FirstName", found.FirstName);
				ViewBag.Details.Add("LastName", found.LastName);
				ViewBag.Details.Add("Telephone", string.Format("{0}", found.Telephone));
				ViewBag.Details.Add("Email", found.Email);
				ViewBag.Details.Add("Message", found.Message);				
			}
			return View(found);
		}

		//Inquiry Edit
		[Authorize(Roles = "Admin")]
		public ViewResult Update(int ID) =>
		View(repository.Inquirys.FirstOrDefault(r => r.ID == ID));
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult Update(Inquiry inquiry)
		{
			if (ModelState.IsValid)
			{
				repository.SaveInquiry(inquiry);
				ViewBag.Message = "Inquiry " + inquiry.ID + " has been edited and saved. ";
				return View("actionResult");
			}
			else
			{
				// there is something wrong with the data values
				return View(inquiry);
			}
		}
	}
}
