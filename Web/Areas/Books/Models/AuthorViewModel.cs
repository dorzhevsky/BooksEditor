using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Books.Models
{
    public class AuthorViewModel
    {
        [Required(ErrorMessage = "Введите имя автора")]
        [StringLength(30, ErrorMessage = "Имя автора не должно превышать {1} символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию автора")]
        [StringLength(30, ErrorMessage = "Фамилия автора не должно превышать {1} символов")]
        public string Surname { get; set; }
    }
}