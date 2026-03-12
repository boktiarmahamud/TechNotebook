using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechNotebook.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Postes_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Postes_Categories_CategoryId",
                table: "Postes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Postes",
                table: "Postes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Postes",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Postes_CategoryId",
                table: "Posts",
                newName: "IX_Posts_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "General" },
                    { 2, null, "Programming" },
                    { 3, null, "DevOps" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "FeaturedImageUrl", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Admin", 1, "This is your first post. Edit or delete it, then start blogging!", "image.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Welcome to TechNotebook!" },
                    { 2, "Admin", 2, "C# is a versatile programming language developed by Microsoft. It is widely used for building a variety of applications, including web, desktop, mobile, and game development. In this post, we'll cover the basics of C# and how to get started with it.", "image.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Getting Started with C#" },
                    { 3, "Admin", 3, "DevOps is a set of practices that combines software development (Dev) and IT operations (Ops). It aims to shorten the software development lifecycle and provide continuous delivery with high software quality. In this post, we'll explore the core principles of DevOps and how it can benefit your organization.", "image.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to DevOps" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Posts_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Posts_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Postes");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CategoryId",
                table: "Postes",
                newName: "IX_Postes_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Postes",
                table: "Postes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Postes_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Postes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Postes_Categories_CategoryId",
                table: "Postes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
