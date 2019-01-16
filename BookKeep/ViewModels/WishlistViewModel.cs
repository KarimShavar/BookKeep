using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Data;
using BookKeep.Models;

namespace BookKeep.ViewModels
{
    public class WishlistViewModel : BaseViewModel
    {
        public WishlistViewModel()
        {
            WishlistCollection = new ObservableCollection<BookModel>(GetWishlist());
        }

        private List<BookModel> GetWishlist()
        {
            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDB(context);
                return LibraryDb.GetAllBooksUnRead();
            }
        }
    }
}
