using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace YummyMummy.Models
{
	//ERD, one entity, one class
	//Ingredient class
	public class Ingredient
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public List<RecipeIngredient> RecipeIngredients { get; set; }
		
		// Ingredient constructor1 
		public Ingredient()
		{
		}

		// Ingredient constructor2	
		public Ingredient(int ID,string Name)
		{
			this.ID = ID;
			this.Name = Name;			
		}

		// ingredient constructor3
		public Ingredient(String Name)
		{
			this.Name = Name;
		}
	}//end of Ingredient Class

}