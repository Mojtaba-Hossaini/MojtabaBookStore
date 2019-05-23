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
    public class CitiesController : Controller
    {
        private readonly BookStoreDb context;

        public CitiesController(BookStoreDb context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
                return NotFound();

            var province = context.Provinces.SingleAsync(c => c.ProvinceID == id);
            context.Entry(await province).Collection(c => c.Cities).Load();
            return View(province.Result);
        }
    }
}