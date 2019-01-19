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
        private ObservableCollection<BookModel> _wishlistCollection;
        public ObservableCollection<BookModel> WishlistCollection
        {
            get => _wishlistCollection;
            set
            {
                _wishlistCollection = value;
                OnPropertyChanged();
            }
        }
        public WishlistViewModel()
        {
            WishlistCollection = new ObservableCollection<BookModel>(GetWishlist());
            DeleteBookCommand = new RelayCommand<BookModel>(OnDelete);
        }

        public override void OnDelete(BookModel book)
        {
            base.OnDelete(book);
            WishlistCollection.Remove(book);
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
