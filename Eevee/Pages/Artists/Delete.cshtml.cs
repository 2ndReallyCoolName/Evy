using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;

namespace Eevee.Pages.Artists
{
    public class DeleteModel : PageModel
    {
        private readonly Eevee.Data.ArtistDataAccess _access;

        public DeleteModel(Eevee.Data.EeveeContext context)
        {
            _access = new ArtistDataAccess(context);
        }

        [BindProperty]
        public Artist Artist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artist = await _access.Get(id.GetValueOrDefault()).FirstOrDefaultAsync();

            if (Artist == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _access.Delete(id.GetValueOrDefault());

            return RedirectToPage("./Index");
        }
    }
}
