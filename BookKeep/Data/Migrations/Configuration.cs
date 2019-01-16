using BookKeep.Models;

namespace BookKeep.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookKeep.Data.BookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Data\Migrations";
        }

        protected override void Seed(BookKeep.Data.BookContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Books.AddOrUpdate(new BookModel()
            {
                Id = 1,
                BookId = 9999,
                Title = "Hobbit",
                Author = "J.J.R Tolkien",
                Description = "In a hole in the ground there lived a hobbit. " +
                              "Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, " +
                              "nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: " +
                              "it was a hobbit-hole, and that means comfort.",
                ImageUrl = "https://images.gr-assets.com/books/1372847500l/5907.jpg",
                IsRead = true
            });
            base.Seed(context);
        }
    }
}
