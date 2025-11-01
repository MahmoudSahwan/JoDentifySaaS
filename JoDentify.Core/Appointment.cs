using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class Appointment : BaseEntity
    {
        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        [StringLength(200, ErrorMessage = "Title must be less than 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Notes must be less than 1000 characters.")]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid status value.")]
        public AppointmentStatus Status { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        [Required]
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public ApplicationUser Doctor { get; set; } = null!;

        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}