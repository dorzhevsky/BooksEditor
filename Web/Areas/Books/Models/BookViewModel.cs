using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Infrastructure;

namespace Web.Areas.Books.Models
{   
    public class BookViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Введите название книги")]
        [StringLength(30, ErrorMessage = "Название не должно превышать {1} символов")]
        public string Title { get; set; }

        [Range(1800, int.MaxValue, ErrorMessage = "Год публикации должен быть не меньше {1}")]
        public int? PublicationYear { get; set; }    

        [Required(ErrorMessage = "Введите количество страниц")]
        [Range(0, 10000, ErrorMessage = "Количество страниц должно быть не меньше {1} и не больше {2}")]
        public int Pages { get; set; }

        [StringLength(30, ErrorMessage = "Название издательства не должно превышать {1} символов")]
        public string Publication { get; set; }

        [Isbn(ErrorMessage = "Значение ISBN не соответствует ISBN формату")]
        public string Isbn { get; set; }

        [NonEmptyList]
        public IEnumerable<AuthorViewModel> Authors { get; set; }
    }
}