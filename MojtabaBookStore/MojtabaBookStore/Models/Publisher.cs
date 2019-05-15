using System.Collections.Generic;

namespace MojtabaBookStore.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public List<Book> Books { get; set; }
    }
}
