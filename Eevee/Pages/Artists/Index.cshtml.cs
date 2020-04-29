using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Eevee.Data;
using Eevee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eevee.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private readonly Eevee.Data.ArtistDataAccess _access;
        public IndexModel(Eevee.Data.EeveeContext context)
        {
            _access = new ArtistDataAccess(context);
        }

        public IList<Artist> Artist { get; set; }

        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }

        public SelectList genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string instrument { get; set; }


        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Artist = await _access.Get(searchString).ToListAsync();
            }
            else
            {
                Artist = await _access.Get("").ToListAsync();
            }
        }
    }
}
