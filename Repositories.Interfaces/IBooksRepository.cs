using System.Collections.Generic;
using Domain;

namespace Repositories.Interfaces
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetAll();
        void Create(Book book);
        void Update(Book book);
        void Delete(int id);
        byte[] GetImage(int id);
    }
}