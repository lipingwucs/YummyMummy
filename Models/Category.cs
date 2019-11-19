using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace YummyMummy.Models
{
	//ERD, one entity, one class
	//Category class
	public class Category
	{
		public int ID { get; set; }
		public string Name { get; set; }
		//public List<RecipeCategory> RecipeCategories { get; set; }//if recipe-Category Many-Many	

		// Category constructor1
		public Category()
		{
		}

		// Category constructor2
		public Category(string Name)
		{
			this.Name = Name;			
		}

		public Category(int CatID, string Name)
		{
			this.ID = CatID;
			this.Name = Name;
		}
	}//end of Category Class

	

}