// المسار: JoDentify.Application/DTOs/Billing/InvoiceItemResponseDto.cs

using System;

namespace JoDentify.Application.DTOs.Billing
{
    public class InvoiceItemResponseDto
    {
        public Guid Id { get; set; } // ID الخاص بالـ item
        public Guid ClinicServiceId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}