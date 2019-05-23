using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Author
    {
        private ILazyLoader LazyLoader { get; set; }
        private List<Author_Book> _Author_Books;
        public Author()
        {

        }

        private Author(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Key]
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Author_Book> Author_Books
        {
            get => LazyLoader.Load(this, ref _Author_Books);
            set => _Author_Books = value;
        }
        
    }
}
