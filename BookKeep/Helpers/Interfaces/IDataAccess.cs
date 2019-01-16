using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Data;
using BookKeep.Models;

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
