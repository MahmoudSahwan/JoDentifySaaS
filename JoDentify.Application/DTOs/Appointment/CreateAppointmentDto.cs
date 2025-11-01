using JoDentify.Core;
using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Appointment
{
    public class CreateAppointmentDto
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

        [Required(ErrorMessage = "PatientId is required.")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        public string DoctorId { get; set; } = string.Empty;
    }
}