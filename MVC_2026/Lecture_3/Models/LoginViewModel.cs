using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true",
            ErrorMessage = "You must agree to the terms and conditions.")]
        public bool AgreeToTerms { get; set; }
    }
}
