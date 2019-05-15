using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Author_Book> Author_Books { get; set; }
    }
}
