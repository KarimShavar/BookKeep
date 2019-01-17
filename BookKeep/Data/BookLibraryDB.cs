using BookKeep.Helpers.Interfaces;
using BookKeep.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeep.Data
{
    public class BookLibraryDb : IDataAccess
    {
        private readonly BookContext _context;

        public BookLibraryDb(BookContext context)
        {
            _context = context;
        }

        public void AddBook(BookModel book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void DeleteBook(long bookId)
        {
            var result = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (result == null) return;

            _context.Books.Remove(result);
            _context.SaveChanges();
        }

        public void UpdateBook(BookModel book)
        {
            var result = _context.Books.FirstOrDefault(b => b.BookId == book.BookId);
            if (result == null) return;

            result.Author = book.Author;
            result.Description = book.Description;
            result.ImageUrl = book.ImageUrl;
            result.IsRead = book.IsRead;
            result.Title = book.Title;

            _context.SaveChanges();
        }

        public BookModel GetBook(long bookId)
        {
            var result = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            return result;
        }

        // Differentiate Read/Unread for easy of accessing Library vs Wishlist
        public List<BookModel> GetAllBooksRead()
        {
            return _context.Books.Where(b => b.IsRead).ToList();
        }

        public List<BookModel> GetAllBooksUnRead()
        {
            return _context.Books.Where(b => b.IsRead == false).ToList();
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public void MarkBookAsRead(long bookId)
        {
            _context.Books.FirstOrDefault(b => b.BookId == bookId).IsRead = true;
            _context.SaveChanges();
        }
    }
}
