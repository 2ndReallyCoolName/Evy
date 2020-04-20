using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eevee.Data;
using Eevee.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Eevee.Pages.Users
{
    public class SignInModel : PageModel
    {
        private readonly Eevee.Data.EeveeContext _context;

        public SignInModel(Eevee.Data.EeveeContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string user_info{ get; set; }

        [BindProperty]
        public string user_password { get; set; }

        public string msg;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = from u in _context.User
                       where u.Email == user_info || u.Username == user_info
                       select u;

            int user_id = user.Select(u => u.UserID).FirstOrDefault();

            var accountType = from a in _context.UserAccountTypeAssignment
                              where a.User.UserID == user_id
                              select a;

            int accountType_id = accountType.Select(a => a.AccountType.AccountTypeID).FirstOrDefault();

            var pass = user.Select(u => u.Password).FirstOrDefault();


            if(!user.Any())
            {
                msg = "No user matches that description.";
                return Page();
            }

            if (pass != user_password)
            {
                msg = "Wrong password.";
                return Page();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim("UserID", user_id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Select(u => u.Username).FirstOrDefault()));
            identity.AddClaim(new Claim("AccountTypeID", accountType_id.ToString()));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });


            return RedirectToPage("/Index");
            
        }
    }

}