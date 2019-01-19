using BookKeep.Data;
using BookKeep.Models;
using BookKeep.ViewModels.Commands;
using Goodreads;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Goodreads.Models.Response;

namespace BookKeep.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        // Todo - Think where should this be based if app extends to require more api functionality.
        private const string ApiKey = "2zS0u37tZAqDBonnsYpSg"; 
        private const string ApiSecret = "ez5q1lv9FCKBkwctFXZbylkXt3Lwn3rFtnc16sqqM";
        private readonly IGoodreadsClient _goodreadsClient;
        private ObservableCollection<BookModel> _searchResults;

        public ObservableCollection<BookModel> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand<BookModel> AddToLibraryCommand { get; private set; }
        public RelayCommand<BookModel> AddToWishlistCommand { get; private set; }

        public SearchViewModel()
        {
            _goodreadsClient = GoodreadsClient.Create(ApiKey, ApiSecret);

            SearchResults = new ObservableCollection<BookModel>();

            AddToLibraryCommand = new RelayCommand<BookModel>(OnAddToLibrary);
            AddToWishlistCommand = new RelayCommand<BookModel>(OnAddToWishlist);
        }

        private void OnAddToWishlist(BookModel book)
        {
            if (book == null) return;
            SetReadFlag(book, false);
            SanitizeBookContent(book);
            book.Description = GetBookDescriptions(book.BookId);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.AddBook(book);
            }
        }
       
        private void OnAddToLibrary(BookModel book)
        {
            if (book == null) return;
            SetReadFlag(book, true);
            SanitizeBookContent(book);
            book.Description = GetBookDescriptions(book.BookId);

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.AddBook(book);
            }
        }

        private void SetReadFlag(BookModel book, bool isRead) => book.IsRead = isRead;

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

        // Todo - fast but could be made async - async command needed.
        private string GetBookDescriptions(long bookId)
        {
            var book = _goodreadsClient.Books.GetByBookId(bookId).Result;
            return book.Description;
        }

        public async Task SearchBooksAsync(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter)) return;
            if (parameter.Length < 2) return; // API limitation on search parameters

            SearchResults.Clear();

            var books = await _goodreadsClient.Books.Search(parameter);
            if (books.Pagination.TotalItems == 0) return;

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
    }
}
