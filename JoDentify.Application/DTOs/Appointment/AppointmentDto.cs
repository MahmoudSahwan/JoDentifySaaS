namespace JoDentify.Application.DTOs.Appointment
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public string DoctorId { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}