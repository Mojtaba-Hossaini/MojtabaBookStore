using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TranslatorsController : Controller
    {
        private readonly BookStoreDb context;

        public TranslatorsController(BookStoreDb context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await context.Translators.ToListAsync());
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
                context.Add(translator);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(translator);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var translator = await context.Translators.FindAsync(id);
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
                context.Update(translator);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(translator);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var translator = await context.Translators.FindAsync(id);
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

            var translator = await context.Translators.FindAsync(id);
            if (translator != null)
            {
                context.Translators.Remove(translator);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}