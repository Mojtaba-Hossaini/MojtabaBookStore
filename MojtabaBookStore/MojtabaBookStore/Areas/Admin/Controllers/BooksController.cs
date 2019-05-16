using System.Linq;
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
    }
}