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
    }
}