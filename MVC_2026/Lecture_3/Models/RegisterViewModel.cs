using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter your name.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter your email.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Create a password.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must contain at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true",
            ErrorMessage = "You must agree to the terms and conditions.")]
        public bool AgreeToTerms { get; set; }
    }
}
