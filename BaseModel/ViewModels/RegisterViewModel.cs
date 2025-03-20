using System.ComponentModel.DataAnnotations;

namespace BaseModel.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required DateTime BirthDate { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }

    }
}
