using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Models;
using MojtabaBookStore.Models.Repository;
using MojtabaBookStore.Models.UnitOfWork;

namespace MojtabaBookStore.Areas.Admin.Pages.Publishers
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork uw;
        private readonly IBaseRepository<Publisher> repo;

        public EditModel(IUnitOfWork uw)
        {
            this.uw = uw;
            repo = uw.BaseRepository<Publisher>();
        }

        [BindProperty]
        public Publisher Publisher { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IsExitPublisher = await repo.FindByID(id);
            if (IsExitPublisher != null)
            {
                Publisher = await repo.FindByID(id);
                return Page();
            }

            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            repo.Update(Publisher);

            try
            {
                await uw.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (repo.FindByID(Publisher.PublisherID) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        
    }
}
