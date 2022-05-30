using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = _userManager.Users;

            List<InputUser> usersOutput = new List<InputUser>();
            foreach (var user in users)
            {
                var newUser = new InputUser();
                newUser.Username = user.UserName;
                newUser.Email = user.Email;
                var claims = await _userManager.GetClaimsAsync(user);
                newUser.Name = claims.First(x => x.Type == "name").Value;
                usersOutput.Add(newUser);
            }
            if (users == null)
            {
                return NotFound();
            } else
            {
                return Ok(usersOutput);
            }
        }
    }
}
