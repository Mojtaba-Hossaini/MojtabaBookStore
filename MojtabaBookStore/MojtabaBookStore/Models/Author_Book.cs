namespace MojtabaBookStore.Models
{
    public class Author_Book
    {
        public int Author_BookID { get; set; }
        public int BookID { get; set; }
        public int AuthorID { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
