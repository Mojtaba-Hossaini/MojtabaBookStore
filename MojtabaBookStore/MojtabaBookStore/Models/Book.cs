using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models
{
    [Table("BookInfo")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string File { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public int LanguageID { get; set; }

        [ForeignKey("SubCategory")]
        public int SCategoryID { get; set; }

        public SubCategory SubCategory { get; set; }
        public Language Language { get; set; }

        public Discount Discount { get; set; }
        public List<Author_Book> Author_Books { get; set; }
        public List<Order_Book> Order_Books { get; set; }
    }
}
