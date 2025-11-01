using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must be less than 100 characters.")]
        public string Email { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be less than 50 characters.")]
        public string UserName { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name must be less than 100 characters.")]
        public string FullName { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Clinic name is required.")]
        [StringLength(100, ErrorMessage = "Clinic name must be less than 100 characters.")]
        public string ClinicName { get; set; } = string.Empty; 
    }
}