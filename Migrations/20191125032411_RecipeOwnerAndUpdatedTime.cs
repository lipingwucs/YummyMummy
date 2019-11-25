using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace YummyMummy.Migrations
{
    public partial class RecipeOwnerAndUpdatedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Recipes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Recipes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "RecipeReviews",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Recipes");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "RecipeReviews",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
