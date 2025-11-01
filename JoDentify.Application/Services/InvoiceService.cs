using AutoMapper;
using JoDentify.Application.DTOs.Billing;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JoDentify.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public InvoiceService(
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;

            var user = httpContextAccessor.HttpContext?.User;
            var clinicIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "clinicId");
            if (clinicIdClaim != null && Guid.TryParse(clinicIdClaim.Value, out Guid clinicId))
            {
                _clinicId = clinicId;
            }
            else
            {
                _clinicId = null;
            }
        }

        private void CheckClinicAccess()
        {
            if (_clinicId == null)
            {
                throw new UnauthorizedAccessException("User is not associated with a clinic.");
            }
        }

        private async Task<InvoiceDto?> GetFullInvoiceDto(Guid invoiceId)
        {
            var invoice = await _context.Invoices
                .AsNoTracking()
                .Include(i => i.Patient)
                .Include(i => i.InvoiceItems)
                .Include(i => i.PaymentTransactions)
                .FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);

            return _mapper.Map<InvoiceDto?>(invoice);
        }

        public async Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createDto)
        {
            CheckClinicAccess();

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == createDto.PatientId && p.ClinicId == _clinicId.Value);
            if (!patientExists) return null;

            if (createDto.AppointmentId.HasValue)
            {
                var appointmentExists = await _context.Appointments.AnyAsync(a => a.Id == createDto.AppointmentId.Value && a.ClinicId == _clinicId.Value);
                if (!appointmentExists) return null;
            }

            var invoice = _mapper.Map<Invoice>(createDto);
            invoice.ClinicId = _clinicId.Value;
            invoice.TotalAmount = 0;
            invoice.AmountPaid = 0;
            invoice.Status = InvoiceStatus.Draft;

            var invoiceItems = new List<InvoiceItem>();

            foreach (var itemDto in createDto.Items)
            {
                var service = await _context.ClinicServices
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == itemDto.ClinicServiceId && s.ClinicId == _clinicId.Value);

                if (service == null)
                {
                    throw new InvalidOperationException($"Service with ID {itemDto.ClinicServiceId} not found for this clinic.");
                }

                var unitPrice = itemDto.UnitPriceOverride ?? service.Price;
                var totalPrice = unitPrice * itemDto.Quantity;

                var invoiceItem = new InvoiceItem
                {
                    ClinicServiceId = service.Id,
                    ItemName = service.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice,
                    ClinicId = _clinicId.Value,
                    Invoice = invoice
                };
                invoiceItems.Add(invoiceItem);
                invoice.TotalAmount += totalPrice;
            }

            invoice.Status = (invoice.TotalAmount == 0) ? InvoiceStatus.Paid : InvoiceStatus.Pending;

            await _context.Invoices.AddAsync(invoice);
            await _context.InvoiceItems.AddRangeAsync(invoiceItems);

            await _context.SaveChangesAsync();

            return await GetFullInvoiceDto(invoice.Id);
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesForClinicAsync()
        {
            CheckClinicAccess();

            var invoices = await _context.Invoices
                                     .AsNoTracking()
                                     .Where(i => i.ClinicId == _clinicId.Value)
                                     .Include(i => i.Patient)
                                     .OrderByDescending(i => i.IssueDate)
                                     .ToListAsync();

            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId)
        {
            CheckClinicAccess();
            return await GetFullInvoiceDto(invoiceId);
        }

        public async Task<InvoiceDto?> AddPaymentAsync(Guid invoiceId, CreatePaymentDto paymentDto)
        {
            CheckClinicAccess();

            var invoice = await _context.Invoices
                                    .FirstOrDefaultAsync(i =>
                                        i.Id == invoiceId &&
                                        i.ClinicId == _clinicId.Value);

            if (invoice == null) return null;

            var amountDue = invoice.TotalAmount - invoice.AmountPaid;
            if (paymentDto.Amount <= 0 || paymentDto.Amount > amountDue)
            {
                return null;
            }

            var transaction = new PaymentTransaction
            {
                InvoiceId = invoiceId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                PaymentMethod = paymentDto.PaymentMethod,
                Notes = paymentDto.Notes,
                ClinicId = _clinicId.Value
            };

            await _context.PaymentTransactions.AddAsync(transaction);

            invoice.AmountPaid += paymentDto.Amount;

            if (invoice.AmountPaid == invoice.TotalAmount)
            {
                invoice.Status = InvoiceStatus.Paid;
            }
            else
            {
                invoice.Status = InvoiceStatus.PartiallyPaid;
            }

            await _context.SaveChangesAsync();

            return await GetFullInvoiceDto(invoiceId);
        }
    }
}