using System.Collections.Generic;

namespace Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public int Pages { get; set; }
        public string Publication { get; set; }
        public string Isbn { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public byte[] Image { get; set; }
    }
}