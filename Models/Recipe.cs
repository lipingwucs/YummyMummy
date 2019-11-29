using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	//ERD, one entity, one class
	//Recipe
	public class Recipe : BaseEntity
	{
		public int ID { get; set; }

		public string UserName { get; set; }

		[Required]
		public int CategoryID { get; set; }
		public Category Category { get; set; }

		[StringLength(60, MinimumLength = 3)]
		[Required]
		public string Name { get; set; }

		[Range(1, 1000)]
		public int CookingTime { get; set; }

		[Range(1, 10000)]
		public double Cost { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public virtual IEnumerable<RecipeIngredient> RecipeIngredients => lineCollection; // recipe-ingredient Many-Many	
		private List<RecipeIngredient> lineCollection = new List<RecipeIngredient>();

		public ICollection<RecipeReview> RecipeReviews { get; set; } // recipe-review one-many

		//public List<RecipeIngredient> RecipeIngredients { get; set; }
		//public List<RecipeCategory> RecipeCategories { get; set; } //if recipe-Category Many-Many	

		public Dictionary<string, string> Dict
		{
			get
			{
				return this.formfields;
			}
		}
		private Dictionary<string, string> formfields;
		private void initDict()
		{
			this.formfields = new Dictionary<string, string>();
			this.formfields.Add("ID", "Recipe ID");
			this.formfields.Add("Category", "Category ID");
			this.formfields.Add("Name", "Recipe Name");
			this.formfields.Add("Description", "Recipe Description");
			this.formfields.Add("CookingTime", "CookingTime(min)");
			this.formfields.Add("Cost", "Cost (CAD)");
		}

		//recipe constructor1
		public Recipe(int RecipeID, int CategoryID, string Name, 
			int CookingTime, string Description, double Cost)
		{
			this.initDict();
			this.ID = RecipeID;			
			this.CategoryID = CategoryID;
			this.Description = Description;
			this.Name = Name;
			this.CookingTime = CookingTime;
			this.Cost = Cost;
		}

		//recipe constructor2
		public Recipe(int CategoryID, string Name, string Ingredients,
			int CookingTime, double Cost)
			{
			this.initDict();
			this.Name = Name;
				this.CategoryID = CategoryID;			
				this.CookingTime = CookingTime;
				this.Cost = Cost;
			}
		//recipe constructor3
		public Recipe() { this.initDict(); }
	}//end of Recipe class	
}