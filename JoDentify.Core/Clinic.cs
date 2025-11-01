using JoDentify.Core;
using System.ComponentModel.DataAnnotations;

namespace JoDentify.Core
{
    public class Clinic : BaseEntity
    {
        [Required(ErrorMessage = "Clinic name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Clinic name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Address must be less than 500 characters.")]
        public string Address { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number must be less than 20 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string OwnerId { get; set; } = string.Empty;
        public ApplicationUser Owner { get; set; } = null!;

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<ClinicService> ClinicServices { get; set; } = new List<ClinicService>();

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    }
}