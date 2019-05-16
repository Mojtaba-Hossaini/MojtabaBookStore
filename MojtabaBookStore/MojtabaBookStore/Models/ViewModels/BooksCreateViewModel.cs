using System.Collections.Generic;

namespace MojtabaBookStore.Models.ViewModels
{
    public class BooksCreateViewModel
    {
        public BooksCreateViewModel(IEnumerable<TreeViewCategory> viewCategories)
        {
            Categories = viewCategories;
        }
        public IEnumerable<TreeViewCategory> Categories { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string File { get; set; }
        public int NumOfPages { get; set; }
        public short Weight { get; set; }
        public string ISBN { get; set; }
        public bool IsPublish { get; set; }
        public int LanguageID { get; set; }
        public int PublisherID { get; set; }
        public int[] AuthorID { get; set; }
        public int[] TranslatorID { get; set; }
        public int[] CategoryID { get; set; }
    }
}
