using System.ComponentModel.DataAnnotations;

namespace Kliker.Models
{
    public class RegisterViewModel
    {
        [Required()]
        [StringLength(35, MinimumLength = 4)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$")]
        public string Username { get; set; }

        [Required()]
        [EmailAddress()]
        public string Mail { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 10)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}|\[\]\\:'<>?,./~`]).{10,}$")]
        public string Password { get; set; }

        [Required()]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
