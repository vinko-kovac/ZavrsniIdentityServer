using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApp.Pages
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public User Input { get; set; }
        public IActionResult OnGet()
        {
            Input = new User { };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.Button != "register")
            {
                return Redirect("/Index");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, object>();
                    data.Add("username", Input.Username);
                    data.Add("password", Input.Password);
                    data.Add("firstName", Input.FirstName);
                    data.Add("lastName", Input.LastName);
                    data.Add("email", Input.Email);

                    var json = JsonConvert.SerializeObject(data);

                    var rep = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("accept", "application/json");

                    var response = await client.PostAsync("https://localhost:5001/Register/Create", rep);

                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        Debug.WriteLine(response.StatusCode.ToString());
                        Debug.WriteLine(response.ToString());
                        return Redirect("/Register");
                    }
                    return Redirect("/Index");

                }
            }
            else
            {
                throw new Exception("Model not valid.");
                return Redirect("/Register");
            }
        }
    }
}
