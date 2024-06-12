using System.ComponentModel.DataAnnotations;

namespace BASE.Model.IdentityAccess
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}
