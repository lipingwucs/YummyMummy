using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using YummyMummy.Data;

namespace YummyMummy.Models
{
	public class EFRecipeRepository:IRecipeRepository
	{
		private RecipeDbContext context;
		//constructor
		public EFRecipeRepository(RecipeDbContext ctx)
		{
			this.context = ctx;
		}

		public DbSet<Recipe> Recipes => context.Recipes;		
		
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid RecipeFileToken { get; set; }

		// load Recipe with related data
		public Recipe GetRecipe(int ID)
		{
			var found =  this.context.Recipes
				.Include(r => r.Category)
				.Include(r => r.RecipeReviews)
			    .Include(r => r.RecipeIngredients)
			   .ThenInclude(ri => ri.Ingredient)
			   .AsNoTracking()
			   .FirstOrDefault(p => p.ID == ID);
			return found;

		}

		//Recipe create/update   
		public void SaveRecipe(Recipe recipe)
		{
			if (recipe.ID == 0)
			{
				context.Recipes.Add(recipe);    //if recipe is not exist, just add
			}
			else
			{
				Recipe dbEntry = context.Recipes
				.FirstOrDefault(r => r.ID == recipe.ID);
				if (dbEntry != null)
				{
					dbEntry.Name = recipe.Name;
					dbEntry.CategoryID = recipe.CategoryID;
					dbEntry.Description = recipe.Description;
					dbEntry.CookingTime = recipe.CookingTime;
					dbEntry.Cost = recipe.Cost;
				}   //if recipe exist, updated the recipe
			}
			context.SaveChanges();
		}
		//Recipe delete
		public Recipe DeleteRecipe(int recipeID)
		{
			Recipe dbEntry = context.Recipes
			.FirstOrDefault(p => p.ID == recipeID);
			if (dbEntry != null)
			{
				context.Recipes.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}



		public DbSet<Ingredient> Ingredients => context.Ingredients;

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid IngredientFileToken { get; set; }

		// load Ingredient with related data
		public Ingredient GetIngredient(int ID)
		{
			var found = this.context.Ingredients
			   .Include(i => i.RecipeIngredients)
			   .ThenInclude(ri => ri.Recipe)
			   .AsNoTracking()
			   .FirstOrDefault(p => p.ID == ID);
			return found;
		}

		public void SaveIngredient(	Ingredient ingredient)
		{

			if (ingredient.ID == 0)
			{
				context.Ingredients.Add(ingredient);
			}
			else
			{
				Ingredient dbEntry = context.Ingredients
				.FirstOrDefault(p => p.ID == ingredient.ID);
				if (dbEntry != null)
				{
					dbEntry.ID = ingredient.ID;
					dbEntry.Name = ingredient.Name;					
				}
			}
			context.SaveChanges();
		}
		//Ingredient delete
		public Ingredient DeleteIngredient(int ID)
		{
			Ingredient dbEntry = context.Ingredients
			.FirstOrDefault(i => i.ID == ID);
			if (dbEntry != null)
			{
				context.Ingredients.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}

		public IEnumerable<Category> Categories => context.Categories;

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid CategoryFileToken { get; set; }

		// load Ingredient with related data
		public Category GetCategory(int ID)
		{
			var found = this.context.Categories
							.AsNoTracking()
							.FirstOrDefault(p => p.ID == ID);
			return found;
		}

		public void SaveCategory(Category category)
		{
			if (category.ID == 0)
			{
				context.Categories.Add(category);
			}
			else
			{
				Category dbEntry = context.Categories
				.FirstOrDefault(p => p.ID == category.ID);
				if (dbEntry != null)
				{
					dbEntry.ID = category.ID;
					dbEntry.Name = category.Name;					
				}
			}
			context.SaveChanges();
		}

		//Category delete
		public Category DeleteCategory(int ID)
		{
			Category dbEntry = context.Categories
									.FirstOrDefault(p => p.ID == ID);
			if (dbEntry != null)
			{
				context.Categories.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}

		public IEnumerable<Inquiry> Inquirys => context.Inquirys;

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid InquiryFileToken { get; set; }
		 
		public void SaveInquiry(Inquiry inquiry)
		{

			if (inquiry.ID == 0)
			{
				context.Inquirys.Add(inquiry);
			}
			else
			{
				Inquiry dbEntry = context.Inquirys
				.FirstOrDefault(p => p.ID == inquiry.ID);
				if (dbEntry != null)
				{
					dbEntry.ID = inquiry.ID;
					dbEntry.FirstName = inquiry.FirstName;
					dbEntry.LastName = inquiry.LastName;
					dbEntry.Email = inquiry.Email;
					dbEntry.Telephone = inquiry.Telephone;
					dbEntry.Message = inquiry.Message;
				}
			}
			context.SaveChanges();
		}
		//Inquiry delete
		public Inquiry DeleteInquiry(int ID)
		{
			Inquiry dbEntry = context.Inquirys
			.FirstOrDefault(r => r.ID ==ID);
			if (dbEntry != null)
			{
				context.Inquirys.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}


		public IEnumerable<RecipeIngredient> RecipeIngredients => context.RecipeIngredients;

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid RecipeIngredientFileToken { get; set; }

		// load Ingredient with related data
		public RecipeIngredient GetRecipeIngredient(int ID)
		{
			var found = this.context.RecipeIngredients
			   .Include(ri => ri.Recipe)
			   .Include(ri => ri.Ingredient)
			   .AsNoTracking()
			   .FirstOrDefault(p => p.ID == ID);
			return found;

		}

		public RecipeIngredient SaveRecipeIngredient(RecipeIngredient recipeIngredient)
		{
			if (recipeIngredient.ID == 0)
			{
				
				context.RecipeIngredients.Add(recipeIngredient);
			}
			else
			{
				RecipeIngredient dbEntry = context.RecipeIngredients
				.FirstOrDefault(p => p.ID == recipeIngredient.ID);
				if (dbEntry != null)
				{
					dbEntry.RecipeID = recipeIngredient.RecipeID;
					dbEntry.IngredientID = recipeIngredient.IngredientID;
					dbEntry.Amount = recipeIngredient.Amount;
					dbEntry.Unit = recipeIngredient.Unit;
				}
			}
			context.SaveChanges();
			return recipeIngredient;

		}
		//RecipeIngredient delete
		public RecipeIngredient DeleteRecipeIngredient(int ID)
		{
			RecipeIngredient dbEntry = this.GetRecipeIngredient(ID);
			if (dbEntry != null)
			{
				context.RecipeIngredients.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}


		public IEnumerable<RecipeReview> RecipeReviews => context.RecipeReviews;

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("newid()")]
		public Guid RecipeReviewFileToken { get; set; }

		// load Review with related data
		public RecipeReview GetRecipeReview(int ID)
		{
			var found = this.context.RecipeReviews
			   .Include(ri => ri.Recipe)
			   .AsNoTracking()
			   .FirstOrDefault(p => p.ID == ID);
			return found;
		}

		public RecipeReview SaveRecipeReview(RecipeReview recipeReview)
		{
			if (recipeReview.ID == 0)
			{
				context.RecipeReviews.Add(recipeReview);
			}
			else
			{
				RecipeReview dbEntry = context.RecipeReviews
				.FirstOrDefault(p => p.ID == recipeReview.ID);
				if (dbEntry != null)
				{
					dbEntry.RecipeID = recipeReview.RecipeID;
					dbEntry.FirstName = recipeReview.FirstName;
					dbEntry.LastName = recipeReview.LastName;
					dbEntry.Telephone = recipeReview.Telephone;
					dbEntry.Email = recipeReview.Email;
					dbEntry.Message = recipeReview.Message;
				}
			}
			context.SaveChanges();
			return recipeReview;
		}

		//RecipeReview delete
		public RecipeReview DeleteRecipeReview(int ID)
		{
			RecipeReview dbEntry = this.GetRecipeReview(ID);
			if (dbEntry != null)
			{
				context.RecipeReviews.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
		}
	}
}
