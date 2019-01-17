using BookKeep.Data;
using BookKeep.Models;
using BookKeep.ViewModels.Commands;
using Goodreads;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookKeep.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        // Todo - Think where should this be based if app extends to require more api functionality.
        private const string ApiKey = "2zS0u37tZAqDBonnsYpSg"; 
        private const string ApiSecret = "ez5q1lv9FCKBkwctFXZbylkXt3Lwn3rFtnc16sqqM";
        private readonly IGoodreadsClient _goodreadsClient;

        public RelayCommand<BookModel> AddToLibraryCommand { get; private set; }
        public RelayCommand<BookModel> AddToWishlistCommand { get; private set; }

        public SearchViewModel()
        {
            _goodreadsClient = GoodreadsClient.Create(ApiKey, ApiSecret);

            SearchResults = new ObservableCollection<BookModel>();

            AddToLibraryCommand = new RelayCommand<BookModel>(OnAddToLibrary);
            AddToWishlistCommand = new RelayCommand<BookModel>(OnAddToWishlist);
        }

        // Todo - Think how to separate this method - Messy
        private void OnAddToWishlist(BookModel obj)
        {
            if (obj == null) return;

            var newBook = SetBookIsRead(obj, false);
            GetBookDescriptions(obj.BookId, newBook);
            SanitizeBookContent(newBook);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.AddBook(newBook);
            }
        }
       
        // Todo - Think how to separate this method - Messy
        private void OnAddToLibrary(BookModel obj)
        {
            if (obj == null) return;

            var newBook = SetBookIsRead(obj, true);
            GetBookDescriptions(obj.BookId, newBook);
            SanitizeBookContent(newBook);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.AddBook(newBook);
            }
        }

        private BookModel SetBookIsRead(BookModel book, bool isRead)
        {
            var newBook = book;
            newBook.IsRead = isRead;
            return newBook;
        }

        /// <summary>
        /// In principle should remove all html tags from string.
        /// Not fool prof, has downsides but does work.
        /// </summary>
        /// <param name="book"></param>
        private void SanitizeBookContent(BookModel book)
        {
           var htmlRegex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[\^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Compiled);

           book.Description = htmlRegex.Replace(book.Description, string.Empty);
           book.Title = htmlRegex.Replace(book.Title, string.Empty);
           book.Author = htmlRegex.Replace(book.Author, string.Empty);
        }

        // Todo - Think how to prevent it from crashing on massive search
        public async Task SearchBooksAsync(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter)) return;
            if (parameter.Length < 3) return;

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

        /// <summary>
        /// Goodreads search api doesn't provide access to descriptions,
        /// Separate method needed to pass description into model,
        /// Not async - would need async commands - ToDo
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="newBook"></param>
        private void GetBookDescriptions(long bookId, BookModel newBook)
        {
            var book = _goodreadsClient.Books.GetByBookId(bookId).Result;
            newBook.Description = book.Description;
        }

    }
}
