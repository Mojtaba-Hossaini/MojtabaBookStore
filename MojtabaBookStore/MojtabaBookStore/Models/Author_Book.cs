using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MojtabaBookStore.Models
{
    public class Author_Book
    {
        private ILazyLoader LazyLoader { get; set; }
        private Book _Book;
        public Author_Book()
        {

        }

        private Author_Book(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        public int BookID { get; set; }
        public int AuthorID { get; set; }

        public Book Book
        {
            get => LazyLoader.Load(this, ref _Book);
            set => _Book = value;
        }
        public Author Author { get; set; }
        
    }
}
