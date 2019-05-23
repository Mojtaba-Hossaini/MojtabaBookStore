using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models.ViewModels
{
    public class BooksSubCategoriesViewModel
    {
        public BooksSubCategoriesViewModel(List<TreeViewCategory> categories, int[] categoryID)
        {
            Categories = categories;
            CategoryID = categoryID;
        }

        public List<TreeViewCategory> Categories { get; set; }
        public int[] CategoryID { get; set; }
    }
}
