using BookKeep.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookKeep.Helpers.Interfaces
{
    public interface IDataAccess
    {
        void AddBook(BookModel book);
        void DeleteBook(long bookId);
        void UpdateBook(BookModel book);
        BookModel GetBook(long bookId);
        List<BookModel> GetAllBooksRead();
        List<BookModel> GetAllBooksUnRead();
        Task<List<BookModel>> GetAllBooksAsync();
        void MarkBookAsRead(long bookId);
    }
}
