using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YummyMummy.Models
{
	

	public class RecipeReview : BaseEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] //this line
		public int ID { get; set; }

		public int RecipeID { get; set; }
		public Recipe Recipe { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Telephone { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Message { get; set; }

	}


}