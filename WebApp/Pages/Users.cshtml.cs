using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
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
                var cookies = HttpContext.Request.Cookies;
                var cookie = string.Empty;
                foreach (var key in cookies.Keys)
                {
                    cookie += key + "=" + cookies[key] + ";";
                }
                client.DefaultRequestHeaders.Add("cookie", cookie);
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
