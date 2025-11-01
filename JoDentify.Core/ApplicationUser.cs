using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JoDentify.Core;

namespace JoDentify.Core
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        public Guid? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }

        public ICollection<Appointment> AppointmentsAsDoctor { get; set; } = new List<Appointment>();
    }
}