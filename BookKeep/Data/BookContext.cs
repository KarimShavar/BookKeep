using BookKeep.Models;
using System.Data.Entity;

namespace BookKeep.Data
{
    public class BookContext : DbContext
    {
        public BookContext() :base("LibraryDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookContext, Migrations.Configuration>());    
        }

        public virtual DbSet<BookModel> Books { get; set; }
    }
}
