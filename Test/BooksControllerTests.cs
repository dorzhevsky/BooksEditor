using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Web.Areas.Books.Controllers;
using Web.Areas.Books.Models;
using Web.Infrastructure;

namespace Web.Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBooksRepository> mockRepository;
        private BooksController controller;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IBooksRepository>();
            controller = new BooksController(mockRepository.Object);
        }

        [Test]
        public void ShouldGetBooks()
        {
            Book book = new Book
            {
                Id = 1,
                Title = "title",
                Pages = 2,
                Publication = "publication",
                Isbn = "isbn",
                PublicationYear = 2010,
                Authors = new List<Author>
                {
                    new Author {Name = "authorName", Surname = "AuthorSurname"}
                }
            };
            IEnumerable<Book> books = new List<Book> { book };
            mockRepository.Setup(m => m.GetAll()).Returns(books);

            JsonResult result = (JsonResult)controller.GetAll();
            IList<BookViewModel> models = ((IEnumerable<BookViewModel>) result.Data).ToList();

            Assert.AreEqual(1, models.Count);
            BookViewModel model = models[0];

            AssertBookEqualBookViewModel(book, model);
        }

        [Test]
        public void ShouldDeleteBook()
        {
            const int id = 1;

            mockRepository.Setup(m => m.Delete(id));
            JsonResult result = (JsonResult)controller.Delete(id);
            Assert.AreEqual(id, result.Data);
            mockRepository.Verify(m => m.Delete(id));
        }

        [Test]
        public void ShouldGetImageIfExists()
        {
            const int id = 1;
            byte[] image = {0};

            mockRepository.Setup(m => m.GetImage(id)).Returns(image);
            FileContentResult result = (FileContentResult)controller.GetImage(id);
            Assert.AreEqual("img", result.ContentType);
            Assert.AreEqual(image, result.FileContents);
            mockRepository.Verify(m => m.GetImage(id));
        }

        [Test]
        public void ShouldGetImageIfDoesNotExist()
        {
            const int id = 1;

            mockRepository.Setup(m => m.GetImage(id)).Returns<int>(i=> null);
            FileContentResult result = (FileContentResult)controller.GetImage(id);

            Assert.AreEqual("img", result.ContentType);
            Assert.AreEqual(new byte[0], result.FileContents);

            mockRepository.Verify(m => m.GetImage(id));
        }

        [Test]
        public void ShouldUpdateBookWithImage()
        {
            BookViewModel book = new BookViewModel
            {
                Id = 1,
                Title = "title",
                Isbn = "isbn",
                Pages = 100,
                Publication = "publication",
                PublicationYear = 2010,
                Authors = new List<AuthorViewModel>
                {
                    new AuthorViewModel { Name = "authorName", Surname = "authorSurname" }
                }
            };

            byte[] fileContent = new byte[0];

            Book savedBook = null;

            mockRepository.Setup(m => m.Update(It.IsAny<Book>())).Callback<Book>(b =>
            {
                savedBook = b;
                AssertBookEqualBookViewModel(b, book);
                Assert.AreEqual(fileContent, b.Image);
            });

            Mock<HttpPostedFileBase> file = new Mock<HttpPostedFileBase>();
            file.Setup(m => m.InputStream).Returns(new MemoryStream(fileContent));

            JsonResult result = (JsonResult)controller.Save(book, file.Object, true);
            BookViewModel returnModel = (BookViewModel)result.Data;

            AssertBookEqualBookViewModel(savedBook, returnModel);
        }

        [Test]
        public void ShouldCreateBook()
        {
            BookViewModel book = new BookViewModel
            {
                Id = 0,
                Title = "title",
                Isbn = "isbn",
                Pages = 100,
                Publication = "publication",
                PublicationYear = 2010,
                Authors = new List<AuthorViewModel>
                {
                    new AuthorViewModel { Name = "authorName", Surname = "authorSurname" }
                }
            };

            byte[] fileContent = new byte[0];

            Book savedBook = null;

            mockRepository.Setup(m => m.Create(It.IsAny<Book>())).Callback<Book>(b =>
            {
                savedBook = b;
                AssertBookEqualBookViewModel(b, book);
                Assert.AreEqual(fileContent, b.Image);
            });

            Mock<HttpPostedFileBase> file = new Mock<HttpPostedFileBase>();
            file.Setup(m => m.InputStream).Returns(new MemoryStream(fileContent));

            JsonResult result = (JsonResult)controller.Save(book, file.Object, true);
            BookViewModel returnModel = (BookViewModel)result.Data;

            AssertBookEqualBookViewModel(savedBook, returnModel);
        }

        [Test]
        public void ShouldValidation()
        {
            BooksController booksController = new BooksController(null);
            booksController.ModelState.AddModelError("key", "message");

            var result = booksController.Save(null, null, true);

            Assert.IsInstanceOf<JsonValidationError>(result);
        }

        [Test]
        public void ShouldUpdateBookWithoutImage()
        {
            const int id = 1;

            BookViewModel book = new BookViewModel
            {
                Id = id,
            };

            byte[] fileContent = new byte[0];

            mockRepository.Setup(m => m.Update(It.IsAny<Book>())).Callback<Book>(b => Assert.AreEqual(fileContent, b.Image));
            mockRepository.Setup(m => m.GetImage(id)).Returns(fileContent);

            controller.Save(book, null, false);
        }

        private static void AssertBookEqualBookViewModel(Book book, BookViewModel model)
        {
            Assert.AreEqual(book.Id, model.Id);
            Assert.AreEqual(book.Title, model.Title);
            Assert.AreEqual(book.Pages, model.Pages);
            Assert.AreEqual(book.Publication, model.Publication);
            Assert.AreEqual(book.Isbn, model.Isbn);
            Assert.AreEqual(book.PublicationYear, model.PublicationYear);

            Assert.AreEqual(book.Authors.Count(), model.Authors.Count());

            for (int i = 0; i < book.Authors.Count(); i++)
            {
                Assert.AreEqual(book.Authors.ElementAt(i).Name, model.Authors.ElementAt(i).Name);
                Assert.AreEqual(book.Authors.ElementAt(i).Surname, model.Authors.ElementAt(i).Surname);
            }
        }
    }
}