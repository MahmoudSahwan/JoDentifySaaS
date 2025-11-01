using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [StringLength(100)]
        public string UserNameOrEmail { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty; 
    }
}