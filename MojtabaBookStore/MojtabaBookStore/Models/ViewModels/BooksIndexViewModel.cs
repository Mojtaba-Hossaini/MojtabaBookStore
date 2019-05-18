using System;

namespace MojtabaBookStore.Models.ViewModels
{
    public class BooksIndexViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string ISBN { get; set; }
        public string PublisherName { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool? IsPublish { get; set; }
        public string Author { get; set; }
    }
}
