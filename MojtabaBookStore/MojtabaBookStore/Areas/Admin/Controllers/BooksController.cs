using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly BookStoreDb context;
        private readonly BooksRepository repo;

        public BooksController(BookStoreDb context, BooksRepository repo)
        {
            this.context = context;
            this.repo = repo;
        }
        public IActionResult Index(int page = 1,int row = 5, string sortExpression = "Title", string title = "")
        {
            string autherNames = "";
            title = string.IsNullOrEmpty(title) ? "" : title;
            List<BooksIndexViewModel> viewModel = new List<BooksIndexViewModel>();
            List<int> rows = new List<int> { 5,10,15,20,50,100};
            ViewBag.RowID = new SelectList(rows,row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            //var books = context.Books.Join(context.Publishers, p => p.PublisherID, c => c.PublisherID, (p, c) => new
            //{
            //    p,c
            //}).Join(context.Author_Books, a => a.p.BookID, b => b.BookID, (a, b) => new
            //{
            //    a,b
            //}).Join(context.Authors, u => u.b.AuthorID, f => f.AuthorID, (u, f) => new BooksIndexViewModel
            //{
            //    BookID = u.a.p.BookID,
            //    ISBN = u.a.p.ISBN,
            //    IsPublish = u.a.p.IsPublished,
            //    Price = u.a.p.Price,
            //    PublishDate = u.a.p.PublishDate,
            //    Stock = u.a.p.Stock,
            //    Title = u.a.p.Title,
            //    PublisherName = u.a.c.PublisherName,
            //    Author = f.FirstName + " " + f.LastName

            //}).GroupBy(b => b.BookID).Select(g => new { BookID = g.Key,BookGroups = g}).ToList();

            var books = context.Author_Books.Include(b => b.Book).ThenInclude(p => p.Publisher).Include(a => a.Author).Where(c => c.Book.IsDeleted == false && c.Book.Title.Contains(title.Trim())).Select(c => new
            {
                Author = c.Author.FirstName + " " + c.Author.LastName,
                c.BookID,
                c.Book.ISBN,
                c.Book.IsPublished,
                c.Book.Price,
                c.Book.PublishDate,
                c.Book.Publisher.PublisherName,
                c.Book.Stock,
                c.Book.Title

            }).GroupBy(b => b.BookID).Select(g => new { BookID = g.Key, BookGroups = g }).ToList();

            foreach (var item in books)
            {
                autherNames = "";
                foreach (var group in item.BookGroups)
                {
                    if (autherNames == "")
                        autherNames = group.Author;
                    else
                        autherNames = autherNames + " - " + group.Author;
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
                };
                viewModel.Add(vm);
            }

            //var books = context.Books.Join(context.Publishers, p => p.PublisherID, c => c.PublisherID, (p, c) => new BooksIndexViewModel
            //{
            //    BookID = p.BookID,
            //    ISBN = p.ISBN,
            //    IsPublish = p.IsPublished,
            //    Price = p.Price,
            //    PublishDate = p.PublishDate,
            //    Stock = p.Stock,
            //    Title = p.Title,
            //    PublisherName = c.PublisherName
            //}).ToList();

            var pagingModel = PagingList.Create(viewModel, row, page,sortExpression,"Title");
            pagingModel.RouteValue = new RouteValueDictionary { {"row",row }, { "title", title} };
            ViewBag.Categories = repo.GetAllCategories();
            ViewBag.LanguageID = new SelectList(context.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(context.Publishers, "PublisherID", "PublisherName");
            ViewBag.AuthorID = new SelectList(context.Authors.Select(c => new AuthorList { AuthorID = c.AuthorID, NameFamily = c.FirstName + " " + c.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(context.Translators.Select(c => new TranslatorList { TranslatorID = c.TranslatorID, NameFamily = c.Name + " " + c.Family }), "TranslatorID", "NameFamily");

            return View(pagingModel);
        }

        public IActionResult Create()
        {
            var categoris = context.Categories.Where(c => c.ParentCategoryID == null).Select(c => new TreeViewCategory
            {
                id = c.CategoryID,
                title = c.CategoryName
            }).ToList();

            foreach (var item in categoris)
            {
                repo.BindSubCategories(item);
            }

            ViewBag.LanguageID = new SelectList(context.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(context.Publishers, "PublisherID", "PublisherName");
            ViewBag.AuthorID = new SelectList(context.Authors.Select(c => new AuthorList { AuthorID = c.AuthorID, NameFamily = c.FirstName + " " + c.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(context.Translators.Select(c => new TranslatorList { TranslatorID = c.TranslatorID, NameFamily = c.Name + " " + c.Family }), "TranslatorID", "NameFamily");

            BooksCreateViewModel viewModel = new BooksCreateViewModel(categoris);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksCreateViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Author_Book> authors = new List<Author_Book>();
                List<Book_Translator> translators = new List<Book_Translator>();
                List<Book_Category> categories = new List<Book_Category>();
                DateTime? PublishDate = null;
                if (ViewModel.IsPublish == true)
                {
                    PublishDate = DateTime.Now;
                }
                Book book = new Book()
                {
                    IsDeleted = false,
                    ISBN = ViewModel.ISBN,
                    IsPublished = ViewModel.IsPublish,
                    NumOfPages = ViewModel.NumOfPages,
                    Stock = ViewModel.Stock,
                    Price = ViewModel.Price,
                    LanguageID = ViewModel.LanguageID,
                    Summary = ViewModel.Summary,
                    Title = ViewModel.Title,
                    PublishYear = ViewModel.PublishYear,
                    PublishDate = PublishDate,
                    Weight = ViewModel.Weight,
                    PublisherID = ViewModel.PublisherID,
                };

                await context.Books.AddAsync(book);

                if (ViewModel.AuthorID != null)
                {
                    for (int i = 0; i < ViewModel.AuthorID.Length; i++)
                    {
                        Author_Book author = new Author_Book()
                        {
                            BookID = book.BookID,
                            AuthorID = ViewModel.AuthorID[i],
                        };

                        authors.Add(author);
                    }

                    await context.Author_Books.AddRangeAsync(authors);
                }


                if (ViewModel.TranslatorID != null)
                {
                    for (int i = 0; i < ViewModel.TranslatorID.Length; i++)
                    {
                        Book_Translator translator = new Book_Translator()
                        {
                            BookID = book.BookID,
                            TranslatorID = ViewModel.TranslatorID[i],
                        };

                        translators.Add(translator);
                    }

                    await context.Book_Translators.AddRangeAsync(translators);
                }

                if (ViewModel.CategoryID != null)
                {
                    for (int i = 0; i < ViewModel.CategoryID.Length; i++)
                    {
                        Book_Category category = new Book_Category()
                        {
                            BookID = book.BookID,
                            CategoryID = ViewModel.CategoryID[i],
                        };

                        categories.Add(category);
                    }

                    await context.Book_Categories.AddRangeAsync(categories);
                }

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.LanguageID = new SelectList(context.Languages, "LanguageID", "LanguageName");
                ViewBag.PublisherID = new SelectList(context.Publishers, "PublisherID", "PublisherName");
                ViewBag.AuthorID = new SelectList(context.Authors.Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
                ViewBag.TranslatorID = new SelectList(context.Translators.Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
                ViewModel.Categories = repo.GetAllCategories();
                return View(ViewModel);
            }
        }

    }
}