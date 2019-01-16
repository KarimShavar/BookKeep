using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Data;
using BookKeep.Models;
using BookKeep.ViewModels.Commands;
using Goodreads;

namespace BookKeep.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public const string ApiKey = "2zS0u37tZAqDBonnsYpSg"; 
        public const string ApiSecret = "ez5q1lv9FCKBkwctFXZbylkXt3Lwn3rFtnc16sqqM";

        public RelayCommand<BookModel> AddToLibraryCommand { get; private set; }
        public RelayCommand<BookModel> AddToWishlistCommand { get; private set; }

        private IGoodreadsClient _goodreadsClient;


        public SearchViewModel()
        {
            _goodreadsClient = GoodreadsClient.Create(ApiKey, ApiSecret);
            SearchResults = new ObservableCollection<BookModel>();
            AddToLibraryCommand = new RelayCommand<BookModel>(OnAddToLibrary);
            AddToWishlistCommand = new RelayCommand<BookModel>(OnAddToWishlist);
        }

        private void OnAddToWishlist(BookModel obj)
        {
            if (obj == null) return;
            var newBook = obj;
            newBook.IsRead = false;

            GetBookDescriptions(obj.BookId, newBook);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDB(context);
                LibraryDb.AddBook(newBook);
            }
        }

        private void OnAddToLibrary(BookModel obj)
        {
            if (obj == null) return;
            var newBook = obj;
            newBook.IsRead = true;

            GetBookDescriptions(obj.BookId, newBook);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDB(context);
                LibraryDb.AddBook(newBook);
            }
        }


        public async Task SearchBooksAsync(string parameter)
        {
                SearchResults.Clear();

                var books = await _goodreadsClient.Books.Search(parameter);
                foreach (var book in books.List)
                {
                    SearchResults.Add(new BookModel()
                    {
                        BookId = book.BestBook.Id,
                        Title = book.BestBook.Title,
                        Author = book.BestBook.AuthorName,
                        Description = string.Empty,
                        ImageUrl = book.BestBook.ImageUrl,
                        IsRead = false
                    });
                }
        }

        //Todo - how to pass description ?
        public void GetBookDescriptions(long bookId, BookModel newBook)
        {
            var book = _goodreadsClient.Books.GetByBookId(bookId).Result;
            newBook.Description = book.Description;
        }

    }
}
