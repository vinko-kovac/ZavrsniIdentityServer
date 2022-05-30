using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApp.Pages
{
    public class UsersModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public List<InputUser> Users { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Users = new List<InputUser> { new InputUser() };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                var response = await client.GetAsync("https://localhost:5001/User/GetAllUsers");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Redirect("/Index");
                }
                var content = await response.Content.ReadAsStringAsync();
                Users = JsonConvert.DeserializeObject<List<InputUser>>(content);
                return Page();
            }
        }
    }
}
