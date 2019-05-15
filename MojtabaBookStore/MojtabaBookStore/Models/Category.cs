using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public List<SubCategory> SubCategory { get; set; }
    }
}
