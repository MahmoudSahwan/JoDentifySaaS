namespace JoDentify.Application.DTOs.Dashboard
{
    // (موديل لجدول آخر المواعيد)
    public class RecentAppointmentDto
    {
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Service { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}