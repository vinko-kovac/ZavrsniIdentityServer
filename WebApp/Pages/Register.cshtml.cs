using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class RegisterModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("https://localhost:5001/Account/Registration/Index");
        }
    }
}
