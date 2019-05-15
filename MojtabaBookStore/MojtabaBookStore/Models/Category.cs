using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        [ForeignKey("category")]
        public int? ParentCategoryID { get; set; }
        public Category category { get; set; }
        public List<Category> Categories { get; set; }

    }
}
