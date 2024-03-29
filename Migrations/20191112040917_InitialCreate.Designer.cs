﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using YummyMummy.Models;
using YummyMummy.Data;

namespace YummyMummy.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    [Migration("20191112040917_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YummyMummy.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("YummyMummy.Models.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("YummyMummy.Models.Inquiry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Message");

                    b.Property<int>("Telephone");

                    b.HasKey("ID");

                    b.ToTable("Inquirys");
                });

            modelBuilder.Entity("YummyMummy.Models.Recipe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<int>("CookingTime");

                    b.Property<double>("Cost");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("YummyMummy.Models.RecipeIngredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("IngredientID");

                    b.Property<int>("RecipeID");

                    b.Property<string>("Unit");

                    b.HasKey("ID");

                    b.HasIndex("IngredientID");

                    b.HasIndex("RecipeID", "IngredientID")
                        .IsUnique();

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("YummyMummy.Models.RecipeReview", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Message");

                    b.Property<int>("RecipeID");

                    b.Property<string>("Telephone");

                    b.HasKey("ID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeReviews");
                });

            modelBuilder.Entity("YummyMummy.Models.Recipe", b =>
                {
                    b.HasOne("YummyMummy.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YummyMummy.Models.RecipeIngredient", b =>
                {
                    b.HasOne("YummyMummy.Models.Ingredient", "Ingredient")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YummyMummy.Models.Recipe", "Recipe")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YummyMummy.Models.RecipeReview", b =>
                {
                    b.HasOne("YummyMummy.Models.Recipe", "Recipe")
                        .WithMany("RecipeReviews")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
