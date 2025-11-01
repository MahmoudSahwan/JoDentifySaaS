using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class Patient : BaseEntity
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must be less than 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number must be less than 20 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value. Must be Male, Female, or Kids.")]
        public Gender Gender { get; set; }

        [StringLength(500, ErrorMessage = "Address must be less than 500 characters.")]
        public string Address { get; set; } = string.Empty;

        public string MedicalHistory { get; set; } = string.Empty;

        [Required]
        public Guid ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}