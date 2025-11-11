// المسار: JoDentify.Application/DTOs/Billing/InvoiceResponseDto.cs

using System;
using System.Collections.Generic;

namespace JoDentify.Application.DTOs.Billing
{
    public class InvoiceResponseDto
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

        public int Status { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ICollection<InvoiceItemResponseDto> InvoiceItems { get; set; } = new List<InvoiceItemResponseDto>();
        public ICollection<PaymentTransactionDto> PaymentTransactions { get; set; } = new List<PaymentTransactionDto>();
    }
}