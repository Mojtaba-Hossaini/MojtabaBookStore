using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;
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

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 5;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public IEnumerable<Publisher> Publisher { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Count = repo.GetCount();
            Publisher = await repo.GetPaginateResultAsync(CurrentPage,PageSize);
            return Page();
        }
    }
}
