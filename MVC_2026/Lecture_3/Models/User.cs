using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool TermsAccepted { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
