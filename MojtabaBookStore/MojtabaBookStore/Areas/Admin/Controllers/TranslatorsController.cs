using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.UnitOfWork;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TranslatorsController : Controller
    {
        private readonly IUnitOfWork uw;
        private readonly IBaseRepository<Translator> repo;

        public TranslatorsController(IUnitOfWork uw)
        {
            this.uw = uw;
            repo = uw.BaseRepository<Translator>();
        }
        public async Task<IActionResult> Index(int page = 1, int row = 10)
        {
            var translators = repo.FindAllAsync();
            var pagingModel = PagingList.Create(await translators, row, page);
            pagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
            };
            return View(pagingModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Translator translator)
        {
            if (ModelState.IsValid)
            {
                await repo.Create(translator);
                await uw.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(translator);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var translator = await repo.FindByID(id);
            if (translator == null)
                return NotFound();
            return View(translator);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Translator translator)
        {
            if (ModelState.IsValid)
            {
                repo.Update(translator);
                await uw.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(translator);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var translator = await repo.FindByID(id);
            if (translator == null)
                return NotFound();

            return View(translator);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleted(int? id)
        {
            if (id == null)
                return NotFound();

            var translator = await repo.FindByID(id);
            if (translator != null)
            {
                repo.Delete(translator);
                await uw.Commit();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}