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

        // --- (جديد) ضيف السطرين دول ---

        [Range(0, double.MaxValue, ErrorMessage = "Amount paid cannot be negative.")]
        public decimal AmountPaid { get; set; } = 0; // المبلغ المدفوع عند الإنشاء

        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; } // الحالة اللي جاية من الفرونت (Pending = 1, Draft = 0, etc.)
    }
}