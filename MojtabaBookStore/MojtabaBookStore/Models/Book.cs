using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    
    public class Book
    {
        
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string File { get; set; }
        public int NumOfPages { get; set; }
        public short Weight { get; set; }
        public string ISBN { get; set; }

        public byte[] Image { get; set; }
        public int LanguageID { get; set; }

        
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }
        public Language Language { get; set; }

        public Discount Discount { get; set; }
        public List<Author_Book> Author_Books { get; set; }
        public List<Order_Book> Order_Books { get; set; }
        public List<Book_Translator> Book_Translators { get; set; }
        public  Publisher Publisher { get; set; }
    }
}
