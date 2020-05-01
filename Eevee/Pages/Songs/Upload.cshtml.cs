using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eevee.Pages.Songs
{
    public class UploadModel : PageModel
    {
        public string msg { get; set; }

        private IWebHostEnvironment _environment;

        public UploadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {

        }


        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task OnPostAsync()
        {
            var file = Path.Combine(_environment.ContentRootPath, "data", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
        }
    }
}