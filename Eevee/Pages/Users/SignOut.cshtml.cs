using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eevee.Pages.Users
{
    public class SingOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Items["UserID"] = 0;
            HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}