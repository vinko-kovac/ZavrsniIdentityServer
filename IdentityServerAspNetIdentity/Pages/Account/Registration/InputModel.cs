using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Pages.Account.Registration
{
    public class InputModel
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string GivenName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
        public string Button { get; set; }
    }
}
