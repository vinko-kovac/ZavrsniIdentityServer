using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] InputUser inputUser)
        {
            var user = _userManager.FindByNameAsync(inputUser.Username).Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = inputUser.Username,
                    Email = inputUser.Email,
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(user, inputUser.Password).Result;
                if (!result.Succeeded)
                {
                    return BadRequest(null);
                }

                result = _userManager.AddClaimsAsync(user, new Claim[]
                {
                        new Claim(JwtClaimTypes.Name, inputUser.FirstName+" "+inputUser.LastName),
                            new Claim(JwtClaimTypes.GivenName, inputUser.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, inputUser.LastName)
                }).Result;

                if (!result.Succeeded)
                {
                    return BadRequest(null);
                }

                return Created("", user);
            }
            else
            {
                return Conflict();
            }
        }

    }
}
