// المسار: E:\JoDentifySaaS\JoDentify.Application\DTOs\Billing\InvoiceDto.cs

namespace JoDentify.Application.DTOs.Billing
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Guid? AppointmentId { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        // --- (هنا التعديل اللي هيحل مشكلة "الحالة الفاضية") ---
        public int Status { get; set; } // (غيرناها من string لـ int)
                                        // --- (نهاية التعديل) ---

        public string Notes { get; set; } = string.Empty;

        public ICollection<InvoiceItemDto> InvoiceItems { get; set; } = new List<InvoiceItemDto>();
        public ICollection<PaymentTransactionDto> PaymentTransactions { get; set; } = new List<PaymentTransactionDto>();
    }
}