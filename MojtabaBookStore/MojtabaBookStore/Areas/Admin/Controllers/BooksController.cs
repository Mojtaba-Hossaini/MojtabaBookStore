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
            title = string.IsNullOrEmpty(title) ? "" : title;
            List<int> rows = new List<int> { 5,10,15,20,50,100};
            ViewBag.RowID = new SelectList(rows,row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            var pagingModel = PagingList.Create(repo.GetAllBooks(title, "", "", "", "", "", ""), row, page,sortExpression,"Title");
            pagingModel.RouteValue = new RouteValueDictionary { {"row",row }, { "title", title} };
            ViewBag.Categories = repo.GetAllCategories();
            ViewBag.LanguageID = new SelectList(context.Languages, "LanguageName", "LanguageName");
            ViewBag.PublisherID = new SelectList(context.Publishers, "PublisherName", "PublisherName");
            ViewBag.AuthorID = new SelectList(context.Authors.Select(c => new AuthorList { AuthorID = c.AuthorID, NameFamily = c.FirstName + " " + c.LastName }), "NameFamily", "NameFamily");
            ViewBag.TranslatorID = new SelectList(context.Translators.Select(c => new TranslatorList { TranslatorID = c.TranslatorID, NameFamily = c.Name + " " + c.Family }), "NameFamily", "NameFamily");

            return View(pagingModel);
        }

        public IActionResult AdvancedSearch(BooksAdvancedSearch viewModel)
        {
            viewModel.Title = String.IsNullOrEmpty(viewModel.Title) ? "" : viewModel.Title;
            viewModel.ISBN = String.IsNullOrEmpty(viewModel.ISBN) ? "" : viewModel.ISBN;
            viewModel.Publisher = String.IsNullOrEmpty(viewModel.Publisher) ? "" : viewModel.Publisher;
            viewModel.Author = String.IsNullOrEmpty(viewModel.Author) ? "" : viewModel.Author;
            viewModel.Translator = String.IsNullOrEmpty(viewModel.Translator) ? "" : viewModel.Translator;
            viewModel.Category = String.IsNullOrEmpty(viewModel.Category) ? "" : viewModel.Category;
            viewModel.Language = String.IsNullOrEmpty(viewModel.Language) ? "" : viewModel.Language;
            var books = repo.GetAllBooks(viewModel.Title, viewModel.ISBN, viewModel.Language, viewModel.Publisher, viewModel.Author, viewModel.Translator, viewModel.Category);
            return View(books);
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bookInfo = await context.Books.Where(c => c.BookID == id).Include(l => l.Language).Include(p => p.Publisher).FirstOrDefaultAsync();
            ViewBag.Authors = context.Author_Books.Where(c => c.BookID == id).Include(a => a.Author).Select(c => new Author {
                AuthorID = c.AuthorID,
                FirstName = c.Author.FirstName,
                LastName = c.Author.LastName
            }).ToList();

            ViewBag.Translators = context.Book_Translators.Include(c => c.Translator).Where(c => c.BookID == id).Select(c => new Translator {
                Name = c.Translator.Name,
                Family = c.Translator.Family
            }).ToList();

            ViewBag.Categories = context.Book_Categories.Include(c => c.Category).Where(c => c.BookID == id).Select(c => new Category
            {
                CategoryName = c.Category.CategoryName
            }).ToList();
            return View(bookInfo);
            
        }

    }
}