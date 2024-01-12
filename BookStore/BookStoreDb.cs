using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public static class BookStoreDb
    {
        public static int AddBook(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);
            using var db = new LiteDatabase(@"BookStore.db");
            var books = db.GetCollection<Book>();
            return books.Insert(book);
        }

        public static List<Book> GetBooks()
        {
            using var db = new LiteDatabase(@"BookStore.db");
            var books = db.GetCollection<Book>();
            return books.Query().ToList();
        }
        
        public static Book GetBook(int id)
        {
            using var db = new LiteDatabase(@"BookStore.db");
            var books = db.GetCollection<Book>();
            return books.FindById(id);
        }

        public static void UpdateBook(Book book)
        {
            using var db = new LiteDatabase(@"BookStore.db");
            var books = db.GetCollection<Book>();
            books.Update(book);
        }
    }
}
