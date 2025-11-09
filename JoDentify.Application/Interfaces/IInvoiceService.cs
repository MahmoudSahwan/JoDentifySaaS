using JoDentify.Application.DTOs.Billing;

namespace JoDentify.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createDto);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesForClinicAsync();
        Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId);
        Task<InvoiceDto?> AddPaymentAsync(Guid invoiceId, CreatePaymentDto paymentDto);
        Task<InvoiceDto?> UpdateInvoiceAsync(Guid invoiceId, CreateInvoiceDto updateDto);

    }
}