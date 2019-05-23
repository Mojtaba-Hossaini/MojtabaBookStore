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
    public class ProvincesController : Controller
    {
        private readonly BookStoreDb context;

        public ProvincesController(BookStoreDb context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await context.Provinces.ToListAsync());
        }
    }
}