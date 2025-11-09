namespace JoDentify.Application.DTOs.Patient
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty; // (هتجيلنا Male أو Female)
        public DateTime JoinDate { get; set; }
    }
}