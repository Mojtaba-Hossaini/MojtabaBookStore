using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }

        public Category Category { get; set; }
        public List<Book> Books { get; set; }
    }
}
