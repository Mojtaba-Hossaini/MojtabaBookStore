using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models
{
    public class Book_Category
    {
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public Book Book { get; set; }
        public Category Category { get; set; }
    }
}
