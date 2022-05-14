using System.Security.Claims;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Account.Registration
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        /*private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;*/

        [BindProperty]
        public InputModel Input { get; set; }

        public IndexModel(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            /*_signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;*/
        }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (Input.Button != "register")
            {
                return Redirect(Input.ReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(Input.Username).Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = Input.Username,
                        Email = Input.Email,
                        EmailConfirmed = true
                    };

                    var result = _userManager.CreateAsync(user, Input.Password).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, Input.GivenName+" "+Input.FamilyName),
                            new Claim(JwtClaimTypes.GivenName, Input.GivenName),
                            new Claim(JwtClaimTypes.FamilyName, Input.FamilyName)
                    }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    if (Url.IsLocalUrl(Input.ReturnUrl))
                    {
                        return Redirect(Input.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(Input.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
            }
            Input = new InputModel
            {
                ReturnUrl = Input.ReturnUrl
            };

            return Page();

        }

        
    }
}
