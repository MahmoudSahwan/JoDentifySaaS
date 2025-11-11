using AutoMapper;
using JoDentify.Application.DTOs.Billing;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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

        // (دالة مساعدة لإرجاع DTO الكامل)
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

        // (دالة مساعدة لإرجاع DTO لصفحة التحرير)
        private async Task<InvoiceResponseDto?> GetFullInvoiceResponseDto(Guid invoiceId)
        {
            var invoice = await _context.Invoices
               .AsNoTracking()
               .Include(i => i.Patient)
               .Include(i => i.InvoiceItems)
               .FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);

            if (invoice == null) return null;

            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                PatientId = invoice.PatientId,
                PatientName = invoice.Patient?.FullName ?? "N/A",
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                TotalAmount = invoice.TotalAmount,
                AmountPaid = invoice.AmountPaid,
                Status = (int)invoice.Status,
                Notes = invoice.Notes,
                InvoiceItems = invoice.InvoiceItems.Select(item => new InvoiceItemResponseDto
                {
                    Id = item.Id, // (إضافة ID)
                    ClinicServiceId = item.ClinicServiceId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList(),
                PaymentTransactions = new List<PaymentTransactionDto>() // (الفرونت إند سيطلبها بشكل منفصل)
            };
        }

        // (دالة إنشاء فاتورة)
        public async Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createDto)
        {
            CheckClinicAccess();

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == createDto.PatientId && p.ClinicId == _clinicId.Value);
            if (!patientExists) return null;

            var invoice = _mapper.Map<Invoice>(createDto);
            invoice.ClinicId = _clinicId.Value;
            invoice.Status = InvoiceStatus.Draft;
            invoice.TotalAmount = 0;
            invoice.AmountPaid = 0; // (الدفعات تضاف لاحقاً)

            var invoiceItems = new List<InvoiceItem>();
            foreach (var itemDto in createDto.Items)
            {
                var service = await _context.ClinicServices
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == itemDto.ClinicServiceId && s.ClinicId == _clinicId.Value);
                if (service == null)
                {
                    throw new InvalidOperationException($"Service with ID {itemDto.ClinicServiceId} not found.");
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

            invoice.Status = RecalculateInvoiceStatus(invoice.TotalAmount, 0, (InvoiceStatus)createDto.Status);

            await _context.Invoices.AddAsync(invoice);
            await _context.InvoiceItems.AddRangeAsync(invoiceItems);
            await _context.SaveChangesAsync();

            return await GetFullInvoiceDto(invoice.Id);
        }

        // --- (هذا هو الإصلاح الأهم) ---
        public async Task<InvoiceDto?> UpdateInvoiceAsync(Guid invoiceId, CreateInvoiceDto updateDto)
        {
            CheckClinicAccess();
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.PaymentTransactions) // (مهم)
                .FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);

            if (invoice == null) return null;

            // (1) تحديث البيانات الأساسية (باستثناء الدفع والحالة)
            invoice.PatientId = updateDto.PatientId;
            invoice.IssueDate = updateDto.IssueDate.ToUniversalTime();
            invoice.DueDate = updateDto.DueDate.HasValue ? updateDto.DueDate.Value.ToUniversalTime() : null;
            invoice.Notes = updateDto.Notes;

            // (2) مسح البنود القديمة وإضافة الجديدة
            _context.InvoiceItems.RemoveRange(invoice.InvoiceItems);
            invoice.TotalAmount = 0; // إعادة حساب الإجمالي
            var newInvoiceItems = new List<InvoiceItem>();
            foreach (var itemDto in updateDto.Items)
            {
                var service = await _context.ClinicServices
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == itemDto.ClinicServiceId && s.ClinicId == _clinicId.Value);
                if (service == null) throw new InvalidOperationException($"Service {itemDto.ClinicServiceId} not found.");

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
                newInvoiceItems.Add(invoiceItem);
                invoice.TotalAmount += totalPrice;
            }
            await _context.InvoiceItems.AddRangeAsync(newInvoiceItems);

            // (3) إعادة حساب المدفوع والحالة (بناءً على الدفعات الموجودة)
            // (هذه الدالة "لا" تعدل الدفعات، هي فقط تعيد الحساب)
            invoice.AmountPaid = invoice.PaymentTransactions.Sum(t => t.Amount);
            invoice.Status = RecalculateInvoiceStatus(invoice.TotalAmount, invoice.AmountPaid, (InvoiceStatus)updateDto.Status);

            await _context.SaveChangesAsync();
            return await GetFullInvoiceDto(invoiceId);
        }

        private InvoiceStatus RecalculateInvoiceStatus(decimal totalAmount, decimal amountPaid, InvoiceStatus desiredStatus)
        {
            if (desiredStatus == InvoiceStatus.Canceled) return InvoiceStatus.Canceled;
            if (totalAmount == 0) return InvoiceStatus.Draft;
            if (amountPaid >= totalAmount) return InvoiceStatus.Paid;
            if (amountPaid > 0) return InvoiceStatus.PartiallyPaid;
            // (لو الإجمالي أكبر من 0 والمدفوع 0)
            return (desiredStatus == InvoiceStatus.Draft) ? InvoiceStatus.Draft : InvoiceStatus.Pending;
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

        // (إصلاح) تعديل نوع الإرجاع ليتطابق مع الإنترفيس
        public async Task<InvoiceResponseDto?> GetInvoiceByIdAsync(Guid invoiceId)
        {
            CheckClinicAccess();
            return await GetFullInvoiceResponseDto(invoiceId);
        }

        // --- (دوال الدفعات الناقصة - تم إضافتها) ---
        public async Task<IEnumerable<PaymentTransactionDto>> GetPaymentsForInvoiceAsync(Guid invoiceId)
        {
            CheckClinicAccess();
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);
            if (invoice == null) throw new UnauthorizedAccessException("Invoice not found or access denied.");

            var payments = await _context.PaymentTransactions
                .AsNoTracking()
                .Where(p => p.InvoiceId == invoiceId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PaymentTransactionDto>>(payments);
        }

        public async Task<InvoiceResponseDto?> AddPaymentAsync(Guid invoiceId, CreatePaymentDto paymentDto)
        {
            CheckClinicAccess();
            var invoice = await _context.Invoices
                                        .FirstOrDefaultAsync(i =>
                                            i.Id == invoiceId &&
                                            i.ClinicId == _clinicId.Value);
            if (invoice == null) return null;

            // (إعادة حساب المتبقي)
            var amountPaid = await _context.PaymentTransactions
                                .Where(p => p.InvoiceId == invoiceId)
                                .SumAsync(p => p.Amount);
            var amountDue = invoice.TotalAmount - amountPaid;

            if (paymentDto.Amount <= 0 || paymentDto.Amount > amountDue)
            {
                return null;
            }
            var transaction = _mapper.Map<PaymentTransaction>(paymentDto);
            transaction.InvoiceId = invoiceId;
            transaction.ClinicId = _clinicId.Value;

            await _context.PaymentTransactions.AddAsync(transaction);

            // (تحديث الفاتورة)
            invoice.AmountPaid = amountPaid + paymentDto.Amount;
            invoice.Status = RecalculateInvoiceStatus(invoice.TotalAmount, invoice.AmountPaid, invoice.Status);

            await _context.SaveChangesAsync();
            return await GetFullInvoiceResponseDto(invoiceId);
        }

        public async Task<PaymentTransactionDto?> UpdatePaymentAsync(Guid invoiceId, Guid paymentId, CreatePaymentDto paymentDto)
        {
            CheckClinicAccess();
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);
            var payment = await _context.PaymentTransactions.FirstOrDefaultAsync(p => p.Id == paymentId && p.InvoiceId == invoiceId && p.ClinicId == _clinicId.Value);

            if (invoice == null || payment == null) return null;

            decimal oldAmount = payment.Amount;
            _mapper.Map(paymentDto, payment); // (تحديث بيانات الدفعة)

            await _context.SaveChangesAsync(); // (احفظ الدفعة أولاً)

            // (إعادة حساب إجمالي الفاتورة)
            var totalPaid = await _context.PaymentTransactions
                                .Where(p => p.InvoiceId == invoiceId)
                                .SumAsync(p => p.Amount);
            invoice.AmountPaid = totalPaid;
            invoice.Status = RecalculateInvoiceStatus(invoice.TotalAmount, invoice.AmountPaid, invoice.Status);

            await _context.SaveChangesAsync(); // (احفظ الفاتورة)
            return _mapper.Map<PaymentTransactionDto>(payment);
        }

        public async Task DeletePaymentAsync(Guid invoiceId, Guid paymentId)
        {
            CheckClinicAccess();
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClinicId == _clinicId.Value);
            var payment = await _context.PaymentTransactions.FirstOrDefaultAsync(p => p.Id == paymentId && p.InvoiceId == invoiceId && p.ClinicId == _clinicId.Value);

            if (invoice == null || payment == null)
            {
                throw new InvalidOperationException("Payment not found or access denied.");
            }

            _context.PaymentTransactions.Remove(payment);
            await _context.SaveChangesAsync(); // (احذف الدفعة أولاً)

            // (إعادة حساب إجمالي الفاتورة)
            var totalPaid = await _context.PaymentTransactions
                                .Where(p => p.InvoiceId == invoiceId)
                                .SumAsync(p => p.Amount);
            invoice.AmountPaid = totalPaid;
            invoice.Status = RecalculateInvoiceStatus(invoice.TotalAmount, invoice.AmountPaid, invoice.Status);

            await _context.SaveChangesAsync(); // (احفظ الفاتورة)
        }
    }
}