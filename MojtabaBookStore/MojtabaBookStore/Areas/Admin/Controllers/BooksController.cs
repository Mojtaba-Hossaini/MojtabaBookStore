using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.UnitOfWork;
using MojtabaBookStore.Models.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork uw;
        private readonly IBaseRepository<Book> repo;
        

        public BooksController(IUnitOfWork uw)
        {
            this.uw = uw;
            repo = uw.BaseRepository<Book>();
        }
        public IActionResult Index(string msg, int page = 1,int row = 5, string sortExpression = "Title", string title = "")
        {
            if (msg != null)
                ViewBag.Msg = "در ثبت اطلاعات به مشکل برخوردیم لطفا بعدا مجددا تلاش کنید";

            title = string.IsNullOrEmpty(title) ? "" : title;
            List<int> rows = new List<int> { 5,10,15,20,50,100};
            ViewBag.RowID = new SelectList(rows,row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            var pagingModel = PagingList.Create(uw.BookRepository.GetAllBooks(title, "", "", "", "", "", ""), row, page,sortExpression,"Title");
            pagingModel.RouteValue = new RouteValueDictionary { {"row",row }, { "title", title} };
            ViewBag.Categories = uw.BookRepository.GetAllCategories();
            ViewBag.LanguageID = new SelectList(uw.BaseRepository<Language>().FindAll(), "LanguageName", "LanguageName");
            ViewBag.PublisherID = new SelectList(uw.BaseRepository<Publisher>().FindAll(), "PublisherName", "PublisherName");
            ViewBag.AuthorID = new SelectList(uw.BaseRepository<Author>().FindAll().Select(c => new AuthorList { AuthorID = c.AuthorID, NameFamily = c.FirstName + " " + c.LastName }), "NameFamily", "NameFamily");
            ViewBag.TranslatorID = new SelectList(uw.BaseRepository<Translator>().FindAll().Select(c => new TranslatorList { TranslatorID = c.TranslatorID, NameFamily = c.Name + " " + c.Family }), "NameFamily", "NameFamily");

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
            var books = uw.BookRepository.GetAllBooks(viewModel.Title, viewModel.ISBN, viewModel.Language, viewModel.Publisher, viewModel.Author, viewModel.Translator, viewModel.Category);
            return View(books);
        }

        public IActionResult Create()
        {
            var categoris = uw.Context.Categories.Where(c => c.ParentCategoryID == null).Select(c => new TreeViewCategory
            {
                id = c.CategoryID,
                title = c.CategoryName
            }).ToList();

            foreach (var item in categoris)
            {
                uw.BookRepository.BindSubCategories(item);
            }

            ViewBag.LanguageID = new SelectList(uw.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(uw.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.AuthorID = new SelectList(uw.BaseRepository<Author>().FindAll().Select(c => new AuthorList { AuthorID = c.AuthorID, NameFamily = c.FirstName + " " + c.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(uw.BaseRepository<Translator>().FindAll().Select(c => new TranslatorList { TranslatorID = c.TranslatorID, NameFamily = c.Name + " " + c.Family }), "TranslatorID", "NameFamily");

            BooksSubCategoriesViewModel subCategoriesVM = new BooksSubCategoriesViewModel(uw.BookRepository.GetAllCategories(), null);
            BooksCreateEditViewModel viewModel = new BooksCreateEditViewModel(subCategoriesVM);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksCreateEditViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Book_Translator> translators = new List<Book_Translator>();
                List<Book_Category> categories = new List<Book_Category>();
                if (ViewModel.TranslatorID != null)
                    translators = ViewModel.TranslatorID.Select(a => new Book_Translator { TranslatorID = a }).ToList();
                if (ViewModel.CategoryID != null)
                    categories = ViewModel.CategoryID.Select(a => new Book_Category { CategoryID = a }).ToList();

                DateTime? PublishDate = null;
                var trnasAction = await uw.Context.Database.BeginTransactionAsync();

                try
                {
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
                        Author_Books = ViewModel.AuthorID.Select(a => new Author_Book { AuthorID = a}).ToList(),
                        Book_Translators = translators,
                        Book_Categories = categories
                        
                    };

                    await repo.Create(book);

                    await uw.Commit();
                    trnasAction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    return RedirectToAction("Index", new { msg = "failed" });
                }
                
            }
            else
            {
                ViewBag.LanguageID = new SelectList(uw.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
                ViewBag.PublisherID = new SelectList(uw.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
                ViewBag.AuthorID = new SelectList(uw.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
                ViewBag.TranslatorID = new SelectList(uw.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
                ViewModel.SubCategoriesVM = new BooksSubCategoriesViewModel(uw.BookRepository.GetAllCategories(), ViewModel.CategoryID);
                return View(ViewModel);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bookInfo = await uw.Context.Books.Where(c => c.BookID == id).Include(l => l.Language).Include(p => p.Publisher).FirstOrDefaultAsync();
            ViewBag.Authors = uw.Context.Author_Books.Where(c => c.BookID == id).Include(a => a.Author).Select(c => new Author {
                AuthorID = c.AuthorID,
                FirstName = c.Author.FirstName,
                LastName = c.Author.LastName
            }).ToList();

            ViewBag.Translators = uw.Context.Book_Translators.Include(c => c.Translator).Where(c => c.BookID == id).Select(c => new Translator {
                Name = c.Translator.Name,
                Family = c.Translator.Family
            }).ToList();

            ViewBag.Categories = uw.Context.Book_Categories.Include(c => c.Category).Where(c => c.BookID == id).Select(c => new Category
            {
                CategoryName = c.Category.CategoryName
            }).ToList();
            return View(bookInfo);
            
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await repo.FindByID(id);
            book.IsDeleted = true;
            await uw.Commit();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await repo.FindByID(id);
            if (book == null)
                return NotFound();

            var viewModel = uw.Context.Books.Include(l => l.Language).Include(p => p.Publisher).Where(c => c.BookID == id).Select(b => new BooksCreateEditViewModel
            {
                BookID = b.BookID,
                Title = b.Title,
                ISBN = b.ISBN,
                NumOfPages = b.NumOfPages,
                Price = b.Price,
                Stock = b.Stock,
                IsPublish = (bool)b.IsPublished,
                LanguageID = b.LanguageID,
                PublisherID = b.Publisher.PublisherID,
                PublishYear = b.PublishYear,
                Summary = b.Summary,
                Weight = b.Weight,
                RecentIsPublish = (bool)b.IsPublished,
                PublishDate = b.PublishDate,
            }).FirstAsync();

            int[] AuthorsArray = await uw.Context.Author_Books.Where(c => c.BookID == id).Select(a => a.AuthorID).ToArrayAsync();
            int[] TranslatorsArray = await uw.Context.Book_Translators.Where(c => c.BookID == id).Select(a => a.TranslatorID).ToArrayAsync();
            int[] CategoriesArray = await uw.Context.Book_Categories.Where(c => c.BookID == id).Select(a => a.CategoryID).ToArrayAsync();

            viewModel.Result.AuthorID = AuthorsArray;
            viewModel.Result.TranslatorID = TranslatorsArray;
            viewModel.Result.CategoryID = CategoriesArray;

            ViewBag.LanguageID = new SelectList(uw.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(uw.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.AuthorID = new SelectList(uw.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(uw.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
            viewModel.Result.SubCategoriesVM = new BooksSubCategoriesViewModel(uw.BookRepository.GetAllCategories(), CategoriesArray);
            return View(await viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BooksCreateEditViewModel viewModel)
        {
            ViewBag.LanguageID = new SelectList(uw.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(uw.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.AuthorID = new SelectList(uw.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(uw.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
            viewModel.SubCategoriesVM = new BooksSubCategoriesViewModel(uw.BookRepository.GetAllCategories(), viewModel.CategoryID);

            if (!ModelState.IsValid)
            {
                ViewBag.MsgFailed = "اطلاعات فرم نامعتبر است  بعد از اصلاح اطلاعات دوباره تلاش کنید";
                return View(viewModel);
            }
                

            try
            {
                DateTime? publishDate;
                if (viewModel.IsPublish == true && viewModel.RecentIsPublish == false)
                {
                    publishDate = DateTime.Now;
                }
                else if (viewModel.RecentIsPublish == true && viewModel.IsPublish == false)
                {
                    publishDate = null;
                }

                else
                {
                    publishDate = viewModel.PublishDate;
                }


                Book book = new Book
                {
                    BookID = viewModel.BookID,
                    Title = viewModel.Title,
                    ISBN = viewModel.ISBN,
                    NumOfPages = viewModel.NumOfPages,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    IsPublished = viewModel.IsPublish,
                    LanguageID = viewModel.LanguageID,
                    PublisherID = viewModel.PublisherID,
                    PublishYear = viewModel.PublishYear,
                    Summary = viewModel.Summary,
                    Weight = viewModel.Weight,
                    PublishDate = publishDate,
                    IsDeleted = false,
                };

                repo.Update(book);

                var RecentAuthors = await uw.Context.Author_Books.Where(c => c.BookID == viewModel.BookID).Select(c => c.AuthorID).ToArrayAsync();
                var RecentTranslators = await uw.Context.Book_Translators.Where(c => c.BookID == viewModel.BookID).Select(c => c.TranslatorID).ToArrayAsync();
                var RecentCategories = await uw.Context.Book_Categories.Where(c => c.BookID == viewModel.BookID).Select(c => c.CategoryID).ToArrayAsync();

                var DeletedAuthors = RecentAuthors.Except(viewModel.AuthorID);
                var DeletedTranslators = RecentTranslators.Except(viewModel.TranslatorID);
                var DeletedCategories = RecentCategories.Except(viewModel.CategoryID);

                var AddedAuthors = viewModel.AuthorID.Except(RecentAuthors);
                var AddedTranslators = viewModel.TranslatorID.Except(RecentTranslators);
                var AddedCategories = viewModel.CategoryID.Except(RecentCategories);

                if (DeletedAuthors.Count() != 0)
                    uw.BaseRepository<Author_Book>().DeleteRange(DeletedAuthors.Select(a => new Author_Book { AuthorID = a, BookID = viewModel.BookID }).ToList());

                if (DeletedTranslators.Count() != 0)
                    uw.BaseRepository<Book_Translator>().DeleteRange(DeletedTranslators.Select(a => new Book_Translator { TranslatorID = a, BookID = viewModel.BookID }).ToList());

                if (DeletedCategories.Count() != 0)
                    uw.BaseRepository<Book_Category>().DeleteRange(DeletedCategories.Select(a => new Book_Category { CategoryID = a, BookID = viewModel.BookID }).ToList());

                if (AddedAuthors.Count() != 0)
                    await uw.BaseRepository<Author_Book>().CreateRange(AddedAuthors.Select(a => new Author_Book { AuthorID = a, BookID = viewModel.BookID }).ToList());

                if (AddedTranslators.Count() != 0)
                    await uw.BaseRepository<Book_Translator>().CreateRange(AddedTranslators.Select(a => new Book_Translator { TranslatorID = a, BookID = viewModel.BookID }).ToList());

                if (AddedCategories.Count() != 0)
                    await uw.BaseRepository<Book_Category>().CreateRange(AddedCategories.Select(a => new Book_Category { CategoryID = a, BookID = viewModel.BookID }).ToList());


                await uw.Commit();

                ViewBag.MsgSuccess = "ویرایش اطلاعات با موقیت انجام شد";
                return View(viewModel);
            }
            catch (Exception)
            {
                ViewBag.MsgFailed = "به هنگام ویرایش اطلاعات مشکلی به وجود آمد لطفا دوباره تلاش کنید";
                return View(viewModel);
            }
        }

    }
}