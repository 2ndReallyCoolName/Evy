using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eevee.Data;
using Eevee.Models;

namespace Eevee.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        private readonly NaturalLanguage.NN.INN _textprocessor;

        public CreateModel(Eevee.Data.EeveeContext context, NaturalLanguage.NN.INN textprocessor)
        {
            _context = context;
            _textprocessor = textprocessor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User _User { get; set; }

        private UserAccountTypeAssignment UATA { get; set; }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _User.PreferenceVector = NaturalLanguage.vector.VectorSpace.ConvertToString(new float[_textprocessor.GetOutputSize()]);

            _context.User.Add(_User);

            UATA = new UserAccountTypeAssignment
            {
                AccountType = _context.AccountType.FirstOrDefault(a => a.Name == "General"),
                User = _User
            };

            _context.UserAccountTypeAssignment.Add(UATA);

            Playlist playlist = new Playlist { Name = "Main", User = _User };

            _context.Playlist.Add(playlist);


            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
