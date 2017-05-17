using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Domain;
using Repositories.Interfaces;
using Web.Areas.Books.Models;
using Web.Controllers;
using Web.Infrastructure;

namespace Web.Areas.Books.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IBooksRepository booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]        
        public ActionResult GetAll()
        {
            IList<BookViewModel> bookEntities = booksRepository.GetAll().Select(Mapper.Map<Book,BookViewModel>).ToList();
            return Json(bookEntities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            byte[] bytes = booksRepository.GetImage(id);
            return File(bytes ?? new byte[0], "img");
        }

        [HttpPost]
        public ActionResult Save(BookViewModel book, 
            [Bind(Prefix = "Image")] HttpPostedFileBase image,
            [Bind(Prefix = "ImageChanged")] bool imageChanged)
        {
            if (ModelState.IsValid)
            {
                Book bookEntity = Mapper.Map<BookViewModel, Book>(book);
                byte[] imageBytes = null;

                if (imageChanged)
                {
                    if (image != null)
                    {
                        imageBytes = Utils.Utils.StreamToArray(image.InputStream);
                    }                    
                }
                else
                {
                    imageBytes = booksRepository.GetImage(bookEntity.Id);
                }

                bookEntity.Image = imageBytes;

                if (bookEntity.Id > 0)
                {
                    booksRepository.Update(bookEntity);
                }
                else
                {
                    booksRepository.Create(bookEntity);
                }
                return Json(Mapper.Map<Book,BookViewModel>(bookEntity));
            }
            return new JsonValidationError(ModelState);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            booksRepository.Delete(id);
            return Json(id);
        }
    }
}
