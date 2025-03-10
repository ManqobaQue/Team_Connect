using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CompanyPhonebook.Models;
using System.Threading.Tasks;

namespace CompanyPhonebook.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LogInModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add your authentication logic here
            // For example, you can use ASP.NET Core Identity to sign in the user

            return RedirectToPage("/Index");
        }
    }
}
