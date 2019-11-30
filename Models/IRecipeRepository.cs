using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YummyMummy.Models
{
	public interface IRecipeRepository
	{
		DbSet<Recipe> Recipes { get; }
		void SaveRecipe(Recipe recipe);
		Recipe DeleteRecipe(int ID);
		Recipe GetRecipe(int ID);

		DbSet<Ingredient> Ingredients { get; }
		void SaveIngredient(Ingredient ingredient);//create,update
		Ingredient DeleteIngredient(int ID);      //delete
		Ingredient GetIngredient(int ID);        //view details
										     	//view list (finished by DBcontext)

		IEnumerable<Category> Categories { get; }
		void SaveCategory(Category category);
		Category DeleteCategory(int ID);
		Category GetCategory(int ID);

		IEnumerable<Inquiry> Inquirys { get; }
		void SaveInquiry(Inquiry Inquirys);
		Inquiry DeleteInquiry(int ID);

		IEnumerable<RecipeIngredient> RecipeIngredients { get; }
		RecipeIngredient SaveRecipeIngredient(RecipeIngredient recipeIngredient);
		RecipeIngredient DeleteRecipeIngredient(int ID);
		RecipeIngredient GetRecipeIngredient(int ID);

		IEnumerable<RecipeReview> RecipeReviews { get; }
		RecipeReview SaveRecipeReview(RecipeReview recipeReview);
		RecipeReview DeleteRecipeReview(int ID);
		RecipeReview GetRecipeReview(int ID);
	}
}
