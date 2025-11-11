using JoDentify.Application.DTOs.Billing;
using JoDentify.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoDentify.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createDto);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesForClinicAsync();
        Task<InvoiceResponseDto?> GetInvoiceByIdAsync(Guid invoiceId);
        Task<InvoiceDto?> UpdateInvoiceAsync(Guid invoiceId, CreateInvoiceDto updateDto);
        Task<IEnumerable<PaymentTransactionDto>> GetPaymentsForInvoiceAsync(Guid invoiceId);
        Task<InvoiceResponseDto?> AddPaymentAsync(Guid invoiceId, CreatePaymentDto paymentDto);
        Task<PaymentTransactionDto?> UpdatePaymentAsync(Guid invoiceId, Guid paymentId, CreatePaymentDto paymentDto);
        Task DeletePaymentAsync(Guid invoiceId, Guid paymentId);
    }
}