using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YummyMummy.Models;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YummyMummy.Data
{
	public class RecipeDbContext : IdentityDbContext<AppUser>
	{
		public DbSet<AppUser> AppUser { get; set; }

		public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
		{ }

		
		public static async Task CreateAdminAccount(IServiceProvider serviceProvider,
			IConfiguration configuration)
		{

			UserManager<AppUser> userManager =
				serviceProvider.GetRequiredService<UserManager<AppUser>>();
			RoleManager<IdentityRole> roleManager =
				serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			string username = configuration["Data:AdminUser:Name"];
			string email = configuration["Data:AdminUser:Email"];
			string password = configuration["Data:AdminUser:Password"];
			string role = configuration["Data:AdminUser:Role"];

			if (await userManager.FindByNameAsync(username) == null)
			{
				if (await roleManager.FindByNameAsync(role) == null)
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}

				AppUser user = new AppUser
				{
					UserName = username,
					Email = email
				};

				IdentityResult result = await userManager
					.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, role);
				}
			}
		}



		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Inquiry> Inquirys { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredients { get; set; }  //bind table Recipe and Ingredient
		public DbSet<RecipeReview> RecipeReviews { get; set; }  //bind table RecipeReview
	//public DbSet<RecipeCategory> RecipeCategories { get; set; } //bind table Recipe and Category

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
			base.OnModelCreating(modelBuilder);
			

			//modelBuilder.Entity<Category>().ToTable("Category");
			//modelBuilder.Entity<Recipe>().ToTable("Recipe");
			//modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
			EntityTypeBuilder<RecipeIngredient> builder = modelBuilder.Entity<RecipeIngredient>();

			// 添加复合Unique主键
			builder.HasIndex(t => new { t.RecipeID, t.IngredientID }).IsUnique();

			///<summary>
			///
			/// 配置Passage与PassageCategories的一对多关系
			/// 
			/// EFCore中,新增默认级联模式为ClientSetNull
			/// 
			/// 依赖实体的外键会被设置为空，同时删除操作不会作用到依赖的实体上，依赖实体保持不变，同下
			/// 
			/// </summary>

			//配置Passage与PassageCategories的一对多关系
			builder.HasOne(t => t.Recipe)
				   .WithMany(p => p.RecipeIngredients)
				   .HasForeignKey(t => t.RecipeID);

			//配置Category与PassageCategories的一对多关系
			builder.HasOne(t => t.Ingredient)
				   .WithMany(p => p.RecipeIngredients)
				   .HasForeignKey(t => t.IngredientID);

			//recipe-Ingredient
			/*
			modelBuilder.Entity<RecipeIngredient>()
				.HasKey(t => new { t.RecipeID, t.IngID });

			modelBuilder.Entity<RecipeIngredient>()
						.HasOne(recipeIngredient => recipeIngredient.Recipe)
						.WithMany(Recipe => Recipe.RecipeIngredients)
						.HasForeignKey(RecipeIngredient => RecipeIngredient.Recipe);	

			modelBuilder.Entity<RecipeIngredient>()
						.HasOne(recipeIngredient => recipeIngredient.Ingredient)
						.WithMany(ingredient => ingredient.RecipeIngredients)
						.HasForeignKey(recipeIngredient => recipeIngredient.IngID);
*/
		}

		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=YummyMummy;Trusted_Connection=True;MultipleActiveResultSets=true");
		}
	

	}

}


