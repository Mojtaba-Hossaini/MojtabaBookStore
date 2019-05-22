using Microsoft.EntityFrameworkCore;
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
                id = c.CategoryID,
                title = c.CategoryName
            }).ToList();

            foreach (var item in categoris)
            {
                BindSubCategories(item);
            }

            return categoris;
        }
        public void BindSubCategories(TreeViewCategory category)
        {
            var subCategoris = context.Categories.Where(c => c.ParentCategoryID == category.id).Select(c => new TreeViewCategory
            {
                id = c.CategoryID,
                title = c.CategoryName
            }).ToList();

            foreach (var item in subCategoris)
            {
                BindSubCategories(item);
                category.subs.Add(item);
            }
        }

        public List<BooksIndexViewModel> GetAllBooks(string title, string ISBN, string Language, string Publisher, string Author, string Translator, string Category)
        {
            string autherNames = "";
            string trnaslatorsName = "";
            string categoriesName = "";
            List<BooksIndexViewModel> viewModel = new List<BooksIndexViewModel>();
            List<int> rows = new List<int> { 5, 10, 15, 20, 50, 100 };

            //var books = context.Author_Books.Include(b => b.Book).ThenInclude(p => p.Publisher).Include(a => a.Author)
            //    .Include(l => l.Book.Language)
            //    .Where(c => c.Book.IsDeleted == false && c.Book.Title.Contains(title.Trim()) && c.Book.ISBN.Contains(ISBN.Trim()) 
            //    && c.Book.Language.LanguageName.Contains(Language.Trim()) && c.Book.Publisher.PublisherName.Contains(Publisher.Trim())).Select(c => new
            //{
            //    Author = c.Author.FirstName + " " + c.Author.LastName,
            //    c.BookID,
            //    c.Book.ISBN,
            //    c.Book.IsPublished,
            //    c.Book.Price,
            //    c.Book.PublishDate,
            //    c.Book.Publisher.PublisherName,
            //    c.Book.Stock,
            //    c.Book.Title

            //}).Where(a => a.Author.Contains(Author)).GroupBy(b => b.BookID).Select(g => new { BookID = g.Key, BookGroups = g }).ToList();

            var books = (from u in context.Author_Books.Include(b => b.Book).ThenInclude(p => p.Publisher)
                         .Include(a => a.Author)
                         join l in context.Languages on u.Book.LanguageID equals l.LanguageID
                         join s in context.Book_Translators on u.Book.BookID equals s.BookID into bt
                         from bts in bt.DefaultIfEmpty()
                         join t in context.Translators on bts.TranslatorID equals t.TranslatorID into tr
                         from trl in tr.DefaultIfEmpty()
                         join r in context.Book_Categories on u.Book.BookID equals r.BookID into bc
                         from bct in bc.DefaultIfEmpty()
                         join c in context.Categories on bct.CategoryID equals c.CategoryID into cg
                         from cog in cg.DefaultIfEmpty()
                         where (u.Book.IsDeleted == false && u.Book.Title.Contains(title.TrimStart().TrimEnd())
                         && u.Book.ISBN.Contains(ISBN.TrimStart().TrimEnd())
                         && EF.Functions.Like(l.LanguageName, "%" + Language + "%")
                         && u.Book.Publisher.PublisherName.Contains(Publisher.TrimStart().TrimEnd()))
                         select new
                         {
                             Author = u.Author.FirstName + " " + u.Author.LastName,
                             Translator = bts != null ? trl.Name + " " + trl.Family : "",
                             Category = bct != null ? cog.CategoryName : "",
                             l.LanguageName,
                             u.Book.BookID,
                             u.Book.ISBN,
                             u.Book.IsPublished,
                             u.Book.Price,
                             u.Book.PublishDate,
                             u.Book.Publisher.PublisherName,
                             u.Book.Stock,
                             u.Book.Title,
                         }).Where(a => a.Author.Contains(Author) && a.Translator.Contains(Translator) && a.Category.Contains(Category)).GroupBy(b => b.BookID).Select(g => new { BookID = g.Key, BookGroups = g }).ToList(); ;



            foreach (var item in books)
            {
                autherNames = "";
                trnaslatorsName = "";
                categoriesName = "";
                foreach (var group in item.BookGroups.Select(a => a.Author).Distinct())
                {
                    if (autherNames == "")
                        autherNames = group;
                    else
                        autherNames = autherNames + " - " + group;
                }
                foreach (var group in item.BookGroups.Select(a => a.Translator).Distinct())
                {
                    if (trnaslatorsName == "")
                        trnaslatorsName = group;
                    else
                        trnaslatorsName = trnaslatorsName + " - " + group;
                }
                foreach (var group in item.BookGroups.Select(a => a.Category).Distinct())
                {
                    if (categoriesName == "")
                        categoriesName = group;
                    else
                        categoriesName = categoriesName + " - " + group;
                }
                BooksIndexViewModel vm = new BooksIndexViewModel()
                {
                    Author = autherNames,
                    BookID = item.BookID,
                    ISBN = item.BookGroups.First().ISBN,
                    Title = item.BookGroups.First().Title,
                    Price = item.BookGroups.First().Price,
                    IsPublish = item.BookGroups.First().IsPublished,
                    PublishDate = item.BookGroups.First().PublishDate,
                    PublisherName = item.BookGroups.First().PublisherName,
                    Stock = item.BookGroups.First().Stock,
                    Translator = trnaslatorsName,
                    Category = categoriesName,
                    Language = item.BookGroups.First().LanguageName,
                };
                viewModel.Add(vm);
            }
            return viewModel;
        }
    }
}
