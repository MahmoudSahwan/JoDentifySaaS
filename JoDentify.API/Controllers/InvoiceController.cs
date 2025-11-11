using Microsoft.AspNetCore.Mvc;
using JoDentify.Application.Interfaces;
using JoDentify.Application.DTOs.Billing;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoDentify.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesForClinicAsync();
            return Ok(invoices);
        }

        // GET: api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<InvoiceResponseDto>> CreateInvoice(CreateInvoiceDto createDto)
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(createDto);
            if (invoice == null) return BadRequest("Failed to create invoice.");
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        // PUT: api/invoices/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> UpdateInvoice(Guid id, CreateInvoiceDto updateDto)
        {
            var invoice = await _invoiceService.UpdateInvoiceAsync(id, updateDto);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        // --- Routes للـ Payments (الناقصة) ---

        // GET: api/invoices/{id}/payments
        [HttpGet("{id}/payments")]
        public async Task<ActionResult<IEnumerable<PaymentTransactionDto>>> GetPaymentsForInvoice(Guid id)
        {
            try
            {
                var payments = await _invoiceService.GetPaymentsForInvoiceAsync(id);
                return Ok(payments);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Access denied.");
            }
        }

        // POST: api/invoices/{id}/payments
        [HttpPost("{id}/payments")]
        public async Task<ActionResult<InvoiceResponseDto>> AddPayment(Guid id, CreatePaymentDto paymentDto)
        {
            try
            {
                var updatedInvoice = await _invoiceService.AddPaymentAsync(id, paymentDto);
                if (updatedInvoice == null) return BadRequest("Failed to add payment.");
                return Ok(updatedInvoice);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Access denied.");
            }
        }

        // PUT: api/invoices/{id}/payments/{paymentId}
        [HttpPut("{id}/payments/{paymentId}")]
        public async Task<ActionResult<PaymentTransactionDto>> UpdatePayment(Guid id, Guid paymentId, CreatePaymentDto paymentDto)
        {
            try
            {
                var updatedPayment = await _invoiceService.UpdatePaymentAsync(id, paymentId, paymentDto);
                if (updatedPayment == null) return NotFound("Payment not found.");
                return Ok(updatedPayment);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Access denied.");
            }
        }

        // DELETE: api/invoices/{id}/payments/{paymentId}
        [HttpDelete("{id}/payments/{paymentId}")]
        public async Task<IActionResult> DeletePayment(Guid id, Guid paymentId)
        {
            try
            {
                await _invoiceService.DeletePaymentAsync(id, paymentId);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound("Payment not found.");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Access denied.");
            }
        }
    }
}