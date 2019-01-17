using BookKeep.Data;
using BookKeep.Models;
using BookKeep.ViewModels.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookKeep.ViewModels
{
    public class WishlistViewModel : BaseViewModel
    {
        public RelayCommand<BookModel> DeleteBookCommand { get; private set; }
        public WishlistViewModel()
        {
            WishlistCollection = new ObservableCollection<BookModel>(GetWishlist());
            DeleteBookCommand = new RelayCommand<BookModel>(OnDeleteFromWishlist);
        }

        // Todo - Almost same method as in MyLibraryViewModel - abstract?
        private void OnDeleteFromWishlist(BookModel obj)
        {
            if (obj == null) return;

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.DeleteBook(obj.BookId);
            }

            WishlistCollection.Remove(obj);
        }

        private List<BookModel> GetWishlist()
        {
            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                return LibraryDb.GetAllBooksUnRead();
            }
        }
    }
}
