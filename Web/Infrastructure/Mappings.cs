using AutoMapper;
using Domain;
using Web.Areas.Books.Models;

namespace Web.Infrastructure
{
    public class Mappings
    {
        public static void CreateMappings()
        {
            Mapper.CreateMap<Book, BookViewModel>();
            Mapper.CreateMap<BookViewModel, Book>();
            Mapper.CreateMap<Author, AuthorViewModel>();
            Mapper.CreateMap<AuthorViewModel, Author>();
        }         
    }
}