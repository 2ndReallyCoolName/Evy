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
    public class DetailsModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        public DetailsModel(Eevee.Data.EeveeContext context)
        {
            _context = context;
        }

        public User _User { get; set; }

        public AccountType AccountType { get; set; }

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

            AccountType = _context.UserAccountTypeAssignment.Where(u => u.User.UserID == id).Select(u=>u.AccountType).FirstOrDefault();

            return Page();
        }
    }
}
