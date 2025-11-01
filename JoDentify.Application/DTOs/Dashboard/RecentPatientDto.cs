namespace JoDentify.Application.DTOs.Dashboard
{
    public class RecentPatientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}