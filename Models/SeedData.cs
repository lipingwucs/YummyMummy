using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using YummyMummy.Data;

namespace YummyMummy.Models
{
	public class SeedData
	{
		public static void EnsurePopulated(IApplicationBuilder app)
		{
			RecipeDbContext context = app.ApplicationServices
			.GetRequiredService<RecipeDbContext>();
			context.Database.Migrate();

			if (!context.Categories.Any())
			{
				context.Categories.AddRange(
					new Category { Name = "Cereal" },
					new Category { Name = "Vegetable" },
					new Category { Name = "Chicken" },
					new Category { Name = "Beef" },
					new Category { Name = "Pork" },
					new Category { Name = "Seafood" },
					new Category { Name = "Soup" },
					new Category { Name = "Drink" }
				);
				context.SaveChanges();
			}

			if (!context.Ingredients.Any())
			{
				context.Ingredients.AddRange(
					new Ingredient { Name = "Long Rice" },
					new Ingredient { Name = "Calrose Rice" },
					new Ingredient { Name = "Sticky Rice" },
					new Ingredient { Name = "Wheat Flour" },
					new Ingredient { Name = "Dried Noodle" },
					new Ingredient { Name = "Rice Noodle" },
					new Ingredient { Name = "Rice Flour" },
					new Ingredient { Name = "Salt" },
					new Ingredient { Name = "Sugar" },
					new Ingredient { Name = "Onion" },
					new Ingredient { Name = "Garlic" },
					new Ingredient { Name = "Green Onion" },
					new Ingredient { Name = "Garlic" },
					new Ingredient { Name = "Vegetable Oil" },
					new Ingredient { Name = "Olive Oil" },
					new Ingredient { Name = "Butter" },
					new Ingredient { Name = "Milk" },					
					new Ingredient { Name = "Light SoySource" },
					new Ingredient { Name = "Dark SoySource" }
							);
				context.SaveChanges();
			}

			if (!context.Recipes.Any())  //if (context.Recipes.Any()) duplicated insert
			{
				////method 1. Excute Raw database sql command, avoid PK in the table
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (1, 'Egg Fried Rice Test','Rice, Egg, Green Onion, Oil', 30, 5.00 )");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (7, 'Sea food conjee Test', 'Rice, Sea food', 10, 20.5)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (2, 'garlic fried lettuce Test', 'Lettus, Garlic', 5, 10.25)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (2, 'Snow on Fire Mountain Test', 'Tomatoes, sugar', 3, 5.00)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (4, 'Homemade Yogurt Test', 'milk, sugar', 60, 10.25)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (1, 'Egg Fried Noodle Test', 'Noodle, Egg, Green Onion, Oil', 20, 10.00)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (6,  'Chicken Fried mushroom Test', 'chicken, mushroom', 30, 14.25)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (6, 'Chicken soup Test', 'chicken, dates', 30, 14.25)");
				//context.Database.ExecuteSqlCommand("INSERT INTO RECIPES (Category,name,Description,cookingtime,cost) " +
				//				"Values (7, 'Seafood Pasta Test', 'Seafood, pasta', 20, 14.25)");

				////	method 2  constructor 2 in Recipes class
				//context.Recipes.AddRange(
				//new Recipe(1, "Egg Fried Rice Test", "Rice, Egg, Green Onion,Oil", 30, 5.00),
				//new Recipe(7, "Sea food conjee Test", "Rice, Sea food", 10, 20.5),
				//new Recipe(2, "garlic fried lettuce Test", "Lettus, Garlic", 5, 10.25),
				//new Recipe(2, "Snow on Fire Mountain Test", "Tomatoes,sugar", 3, 5.00),
				//new Recipe(4, "Homemade Yogurt Test", "milk,sugar", 60, 10.25),
				//new Recipe(1, "Egg Fried Noodle Test", "Noodle, Egg, Green Onion,Oil", 20, 10.00),
				//new Recipe(6, "Chicken Fried mushroom Test", "chicken, mushroom", 30, 14.25),
				//new Recipe(6, "Chicken soup Test", "chicken, dates", 30, 14.25),
				//new Recipe(7, "Seafood Pasta Test", "Seafood, pasta", 20, 14.25)
				//);
				//context.SaveChanges();

				////construtor Recipes()
				//context.Recipes.AddRange(
				//new Recipe { Name="Egg Fried Rice Test2", Description="Rice, Egg, Green Onion,Oil",CookingTime=30, Cost=5.00},
				//
				//			   	);

				context.Recipes.AddRange(
					new Recipe
					{
						CategoryID = 1,
						Name = "Egg Fried Rice",
						Description = "Rice, Egg, Green Onion,Oil",
						CookingTime = 30,
						Cost = 5.00,
						UserName="Liping"
					},
					//2
					new Recipe
					{
						CategoryID = 1,
						Name = "Meat Fried Rice ",
						Description = "Rice, Meat, Green Onion,Oil",
						CookingTime = 40,
						Cost = 2.00,
						UserName = "Liping"
					},
					//3
					new Recipe
					{
						CategoryID = 1,
						Name = "Seafood Rice Noodle",
						Description = "Rice Noodle, Seafood, Vegetable, Green Onion,Oil",
						CookingTime = 10,
						Cost = 10.00,
						UserName = "Liping"
					},
					//4
					new Recipe
					{
						CategoryID = 2,
						Name = "Stir Fried Season Vegetable",
						Description = "Season Vegetable like lettuce, brocolli, Napa etc",
						CookingTime = 10,
						Cost = 5.00,
						UserName = "Liping"
					},
					//5
					new Recipe
					{
						CategoryID = 3,
						Name = "Deep Fried Chicken",
						Description = "Deep Fried Chicken Breast Meat",
						CookingTime = 10,
						Cost = 5.00,
						UserName = "Liping"
					},
					//6
					new Recipe
					{
						CategoryID = 4,
						Name = "Seasoned Stun Beef",
						Description = "Beef, Salt, Dark Soysauce, Ginger, Pepper",
						CookingTime = 60,
						Cost = 20.00,
						UserName = "Liping"
					},
					//7
					new Recipe
					{
						CategoryID = 5,
						Name = "Pork Stir fry with hot pepper",
						Description = "Pork, Green hot pepper, Dark Soysauce, Ginger",
						CookingTime = 60,
						Cost = 20.00,
						UserName = "Liping"
					},
					//8
					new Recipe
					{
						CategoryID = 6,
						Name = "Steamed Crab",
						Description = "Steam Live Crab, dipped with Light Soysauce, Ginger ",
						CookingTime = 40,
						Cost = 20.00,
						UserName = "Liping"
					},
					//9
					new Recipe
					{
						CategoryID = 7,
						Name = "Chicken Soup",
						Description = "Chicken, mushroom, red dates, ginger, salt",
						CookingTime = 60,
						Cost = 20.00,
						UserName = "Liping"
					},
					//10
					new Recipe
					{
						CategoryID = 8,
						Name = "Home-made blueberry Yogurt",
						Description = "Milk, Yogurt Starter, Sugar, Blueberry",
						CookingTime = 360,
						UserName = "Liping",
						Cost = 5,
					}
					);
					context.SaveChanges();
			}

			if (!context.RecipeIngredients.Any())
			{
				context.RecipeIngredients.AddRange(
					new RecipeIngredient { RecipeID = 1, IngredientID = 1, Amount =250, Unit="g" },
					new RecipeIngredient { RecipeID = 1, IngredientID = 3, Amount = 4, Unit = "piece" },
					new RecipeIngredient { RecipeID = 1, IngredientID = 10, Amount = 2, Unit = "piece" },
					new RecipeIngredient { RecipeID = 1, IngredientID = 14, Amount =100, Unit = "ml" },

					new RecipeIngredient { RecipeID = 2, IngredientID = 1, Amount = 250, Unit = "g" },
					new RecipeIngredient { RecipeID = 2, IngredientID = 4, Amount = 100, Unit = "g" },
					new RecipeIngredient { RecipeID = 2, IngredientID = 10, Amount = 2, Unit = "piece" },
					new RecipeIngredient { RecipeID = 2, IngredientID = 14, Amount = 100, Unit = "ml" },

					new RecipeIngredient { RecipeID = 3, IngredientID = 7, Amount = 300, Unit = "g" },
					new RecipeIngredient { RecipeID = 3, IngredientID = 2, Amount = 12, Unit = "piece" },
					new RecipeIngredient { RecipeID = 3, IngredientID = 8, Amount = 5, Unit = "g" },
					new RecipeIngredient { RecipeID = 3, IngredientID = 14, Amount = 10, Unit = "ml" },

					new RecipeIngredient { RecipeID = 4, IngredientID = 11, Amount = 500, Unit = "g" },
					new RecipeIngredient { RecipeID = 4, IngredientID = 13, Amount = 5, Unit = "piece" },
					new RecipeIngredient { RecipeID = 4, IngredientID = 8, Amount = 5, Unit = "g" },
					new RecipeIngredient { RecipeID = 4, IngredientID = 15, Amount = 10, Unit = "ml" },

					new RecipeIngredient { RecipeID = 5, IngredientID = 6, Amount = 500, Unit = "g" },
					new RecipeIngredient { RecipeID = 5, IngredientID = 8, Amount = 13, Unit = "g" },
					new RecipeIngredient { RecipeID = 5, IngredientID = 14, Amount = 1, Unit = "liter" },

					new RecipeIngredient { RecipeID = 6, IngredientID = 5, Amount = 2, Unit = "kg" },
					new RecipeIngredient { RecipeID = 6, IngredientID = 8, Amount = 50, Unit = "g" },
					new RecipeIngredient { RecipeID = 6, IngredientID = 19, Amount = 50, Unit = "ml" },
					
					new RecipeIngredient { RecipeID = 7, IngredientID = 4, Amount = 1, Unit = "kg" },
					new RecipeIngredient { RecipeID = 7, IngredientID = 8, Amount = 10, Unit = "g" },
					new RecipeIngredient { RecipeID = 7, IngredientID = 10, Amount = 1, Unit = "piece" },
					new RecipeIngredient { RecipeID = 7, IngredientID = 14, Amount = 50, Unit = "ml" },

					new RecipeIngredient { RecipeID = 8, IngredientID = 12, Amount = 12, Unit = "piece" },
					new RecipeIngredient { RecipeID = 8, IngredientID = 18, Amount = 50, Unit = "ml" },
					new RecipeIngredient { RecipeID = 8, IngredientID = 13, Amount = 20, Unit = "g" },

					new RecipeIngredient { RecipeID = 9, IngredientID = 6, Amount = 500, Unit = "g" },
					new RecipeIngredient { RecipeID = 9, IngredientID = 8, Amount = 10, Unit = "g" },
					new RecipeIngredient { RecipeID = 9, IngredientID = 12, Amount = 1, Unit = "piece" },

					new RecipeIngredient { RecipeID = 10, IngredientID = 17, Amount = 2, Unit = "liter" },
					new RecipeIngredient { RecipeID = 9, IngredientID = 9, Amount = 100, Unit = "g" }

				);
				context.SaveChanges();
			}

			if (!context.RecipeReviews.Any())
			{
				context.RecipeReviews.AddRange(
					new RecipeReview { RecipeID = 1, FirstName="Bill", LastName="Gates", Email="bg@hotmail.com", Telephone="888-888-8888", Message="My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 1, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 1, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },
					new RecipeReview { RecipeID = 1, FirstName = "Jason", LastName = "Chen", Email = "jc@hotmail.com", Telephone = "888-888-8888", Message = "I love this very much.  Thanks." },

					new RecipeReview { RecipeID = 2, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 2, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 2, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 3, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 3, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 3, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 4, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 4, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 4, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 5, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 5, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 5, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 6, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 6, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 6, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 7, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 7, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 7, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 8, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 8, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 8, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 9, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 9, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 9, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." },

					new RecipeReview { RecipeID = 10, FirstName = "Bill", LastName = "Gates", Email = "bg@hotmail.com", Telephone = "888-888-8888", Message = "My wife love this very much. Thanks." },
					new RecipeReview { RecipeID = 10, FirstName = "Mark", LastName = "Gates", Email = "mg@hotmail.com", Telephone = "888-888-8888", Message = "My little kids love this very much. Thanks." },
					new RecipeReview { RecipeID = 10, FirstName = "Joe", LastName = "Black", Email = "jb@hotmail.com", Telephone = "888-888-8888", Message = "My grandpa love this very much.  Thanks." }

				);
				context.SaveChanges();
			}

		}
	}
}


