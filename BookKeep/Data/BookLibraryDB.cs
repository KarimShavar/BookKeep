using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Helpers.Interfaces;
using BookKeep.Models;

namespace BookKeep.Data
{
    public class BookLibraryDB : IDataAccess
    {
        private BookContext Context;

        public BookLibraryDB(BookContext context)
        {
            Context = context;
        }

        public void AddBook(BookModel book)
        {
            Context.Books.Add(book);
            Context.SaveChanges();
        }

        public void DeleteBook(long bookId)
        {
            var result = Context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (result == null) return;

            Context.Books.Remove(result);
            Context.SaveChanges();
        }

        public void UpdateBook(BookModel book)
        {
            var result = Context.Books.FirstOrDefault(b => b.BookId == book.BookId);
            if (result == null) return;

            result.Author = book.Author;
            result.Description = book.Description;
            result.ImageUrl = book.ImageUrl;
            result.IsRead = book.IsRead;
            result.Title = book.Title;

            Context.SaveChanges();
        }

        public BookModel GetBook(long bookId)
        {
            var result = Context.Books.FirstOrDefault(b => b.BookId == bookId);
            return result;
        }

        public List<BookModel> GetAllBooksRead()
        {
            if (Context.Books.Any() == false) throw new Exception("Database is empty");
            return Context.Books.Where(b => b.IsRead).ToList();
        }

        public List<BookModel> GetAllBooksUnRead()
        {
            if (Context.Books.Any() == false) throw new Exception("Database is empty");
            return Context.Books.Where(b => b.IsRead == false).ToList();
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            if (Context.Books.Any() == false) throw new Exception("Database is empty");
            return await Context.Books.ToListAsync();
        }

        public void MarkBookAsRead(long bookId)
        {
                Context.Books.FirstOrDefault(b => b.BookId == bookId).IsRead = true;
                Context.SaveChanges();
        }
    }
}
