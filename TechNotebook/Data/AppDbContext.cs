using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechNotebook.Models;

namespace TechNotebook.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Post> Posts { get; set; }
		public DbSet<Category> Categories { get; set; }
        public DbSet<Comments> Comment { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data — use the model property name `Id` (PascalCase)
            base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "General"},
                new Category { Id = 2, Name = "Programming"},
                new Category { Id = 3, Name = "DevOps"}
            );

            modelBuilder.Entity<Post>().HasData(
                 new Post
                 {
                     Id = 1,
                     Title = "Welcome to TechNotebook!",
                     Content = "This is your first post. Edit or delete it, then start blogging!",
                     Author = "Admin",
                     PublishedDate = new DateTime(2024, 1, 1),
                     CategoryId = 1,
                     FeaturedImageUrl = "image.jpg",//sample data
                 },
                 new Post
                 {
                     Id = 2,
                     Title = "Getting Started with C#",
                     Content = "C# is a versatile programming language developed by Microsoft. It is widely used for building a variety of applications, including web, desktop, mobile, and game development. In this post, we'll cover the basics of C# and how to get started with it.",
                     Author = "Admin",
                     PublishedDate = new DateTime(2024, 1, 1),
                     CategoryId = 2,
                     FeaturedImageUrl = "image.jpg",//sample data

				 },
                 new Post
                 {
                     Id = 3,
                     Title = "Introduction to DevOps",
                     Content = "DevOps is a set of practices that combines software development (Dev) and IT operations (Ops). It aims to shorten the software development lifecycle and provide continuous delivery with high software quality. In this post, we'll explore the core principles of DevOps and how it can benefit your organization.",
                     Author = "Admin",
                     PublishedDate = new DateTime(2024, 1, 1),
                     CategoryId = 3,
                     FeaturedImageUrl = "image.jpg",//sample data
				 }

             );
		}
    }
}
