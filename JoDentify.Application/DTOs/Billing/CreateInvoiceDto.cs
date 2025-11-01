using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Billing
{
    public class CreateInvoiceDto
    {
        [Required(ErrorMessage = "PatientId is required.")]
        public Guid PatientId { get; set; }

        public Guid? AppointmentId { get; set; }

        [Required(ErrorMessage = "IssueDate is required.")]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one item is required.")]
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public ICollection<CreateInvoiceItemDto> Items { get; set; } = new List<CreateInvoiceItemDto>();
    }
}