using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MojtabaBookStore.Models;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.ViewModels;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var categoris = context.Categories.Where(c => c.ParentCategoryID == null).Select(c => new TreeViewCategory
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
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