using MojtabaBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models.Repository
{
    public class BooksRepository
    {
        private readonly BookStoreDb context;

        public BooksRepository(BookStoreDb context)
        {
            this.context = context;
        }

        public List<TreeViewCategory> GetAllCategories()
        {
            var categoris = context.Categories.Where(c => c.ParentCategoryID == null).Select(c => new TreeViewCategory
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            }).ToList();

            foreach (var item in categoris)
            {
                BindSubCategories(item);
            }

            return categoris;
        }
        public void BindSubCategories(TreeViewCategory category)
        {
            var subCategoris = context.Categories.Where(c => c.ParentCategoryID == category.CategoryID).Select(c => new TreeViewCategory
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            }).ToList();

            foreach (var item in subCategoris)
            {
                BindSubCategories(item);
                category.SubCategories.Add(item);
            }
        }
    }
}
