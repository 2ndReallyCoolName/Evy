using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eevee.Data;
using Eevee.Models;
using Vspace = NaturalLanguage.vector.VectorSpace;

namespace Eevee.Pages.Artists
{
    public class CreateModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        private readonly NaturalLanguage.NN.INN _textprocessor;

        private readonly Eevee.Data.ArtistDataAccess _access;
        
        public CreateModel(Eevee.Data.EeveeContext context, NaturalLanguage.NN.INN textprocessor)
        {
            _context = context;
            _textprocessor = textprocessor;
            _access = new ArtistDataAccess(context);
        }

        public User _User { get; set; }

        private UserAccountTypeAssignment UATA { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Users/SignIn");
            }

            _User = _context.User.FirstOrDefault(u=>u.UserID == id.GetValueOrDefault());

            return Page();
        }

        public string error { get; set; }

        [BindProperty]
        public Artist Artist { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {

            Artist.ArtistID = id;

            if (!ModelState.IsValid)
            {

                error = string.Join(" | ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));

                error += id.ToString();

                return Page();
            }
            
            Artist.Listens = 0;

            Artist.Rating = 1;

            Artist.WordVec = Vspace.ConvertToString(Vspace.Add(_textprocessor.PredictText(Artist.Name), _textprocessor.PredictText(Artist.Description)));

            await _access.Create(id, Artist);

            return RedirectToPage("./Index");
        }
    }
}
