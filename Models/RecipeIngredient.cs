using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	

	public class RecipeIngredient : BaseEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] //this line
		public int ID { get; set; }

		public int RecipeID { get; set; }
		public Recipe Recipe { get; set; }

		[Required]
		public int IngredientID { get; set; }
		public Ingredient Ingredient { get; set; }
		public string Unit { get; set; }
		public int Amount { get; set; }
	}
}