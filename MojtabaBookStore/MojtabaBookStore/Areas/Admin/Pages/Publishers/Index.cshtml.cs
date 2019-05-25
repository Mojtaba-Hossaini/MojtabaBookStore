using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.UnitOfWork;

namespace MojtabaBookStore.Areas.Admin.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork uw;
        private readonly IBaseRepository<Publisher> repo;

        public IndexModel(IUnitOfWork uw)
        {
            this.uw = uw;
            repo = uw.BaseRepository<Publisher>();
        }

        public IEnumerable<Publisher> Publishers { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Publishers = await repo.FindAllAsync();
            return Page();
        }
    }
}