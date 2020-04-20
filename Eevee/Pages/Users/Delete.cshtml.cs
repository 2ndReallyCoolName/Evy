using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;

namespace Eevee.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        public DeleteModel(Eevee.Data.EeveeContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User _User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _User = await _context.User.FirstOrDefaultAsync(m => m.UserID == id);

            if (_User == null)
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

            _User = await _context.User.FindAsync(id);

            List<UserAccountTypeAssignment> uata = _context.UserAccountTypeAssignment.Where(u => u.User.UserID == _User.UserID).ToList();

            Artist Artist = await _context.Artist.FindAsync(id);

            if (_User != null)
            {
                _context.User.Remove(_User);

                foreach(var ut in uata)
                {
                    _context.UserAccountTypeAssignment.Remove(ut);
                }

                if (Artist != null)
                    _context.Artist.Remove(Artist);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
