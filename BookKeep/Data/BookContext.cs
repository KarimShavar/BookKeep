using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Models;

namespace BookKeep.Data
{
    public class BookContext : DbContext
    {
        public BookContext() :base("LibraryDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookContext, Data.Migrations.Configuration>());    
        }

        public virtual DbSet<BookModel> Books { get; set; }
    }
}
