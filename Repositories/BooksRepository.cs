using System.Collections.Generic;
using System.Linq;
using Domain;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class BooksRepository : IBooksRepository
    {
        private static readonly IDictionary<int,Book> books = new Dictionary<int, Book>
        {
            {1, new Book {Id = 1, Pages = 100, Title = "Pro .NET Performance", PublicationYear = 2010, Authors = new List<Author> 
            {
                new Author {Name = "Sasha", Surname = "Goldstein"}
            }}},
            {2, new Book {Id = 2, Pages = 200, Title = "Working with legacy code", PublicationYear = 2011, Authors = new List<Author> 
            {
                new Author {Name = "Dima", Surname = "Zurbalev"}
            }}}
        }; 

        public IEnumerable<Book> GetAll()
        {
            return books.Values;
        }

        public void Update(Book book)
        {
            if (books.ContainsKey(book.Id))
            {
                books[book.Id] = book;
            }
        }

        public byte[] GetImage(int id)
        {
            return books.ContainsKey(id) ? books[id].Image : new byte[] {};
        }

        public void Create(Book book)
        {
            book.Id = NextId();
            books.Add(book.Id, book);
        }

        public void Delete(int id)
        {
            if (books.ContainsKey(id))
            {
                books.Remove(id);
            }
        }

        private static int NextId()
        {
            return books.Values.Max(e => e.Id) + 1;
        }
    }
}