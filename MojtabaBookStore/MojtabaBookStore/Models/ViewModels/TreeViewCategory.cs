using System.Collections.Generic;

namespace MojtabaBookStore.Models.ViewModels
{
    public class TreeViewCategory
    {
        public TreeViewCategory()
        {
            SubCategories = new List<TreeViewCategory>();
        }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<TreeViewCategory> SubCategories { get; set; }
    }
}
