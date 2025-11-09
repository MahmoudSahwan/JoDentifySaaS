using JoDentify.Application.DTOs.Billing;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoDentify.API.Controllers
{
    [Authorize]
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newInvoice = await _invoiceService.CreateInvoiceAsync(createDto);
                if (newInvoice == null)
                {
                    return BadRequest("Invalid PatientId, AppointmentId, or ServiceId for this clinic.");
                }
                return CreatedAtAction(nameof(GetInvoiceById), new { id = newInvoice.Id }, newInvoice);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetAllInvoicesForClinicAsync();
                return Ok(invoices);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                return Ok(invoice);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("{id:guid}/payments")]
        public async Task<IActionResult> AddPayment(Guid id, [FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedInvoice = await _invoiceService.AddPaymentAsync(id, paymentDto);
                if (updatedInvoice == null)
                {
                    return NotFound("Invoice not found or payment amount is invalid.");
                }
                return Ok(updatedInvoice);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] CreateInvoiceDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedInvoice = await _invoiceService.UpdateInvoiceAsync(id, updateDto);
                if (updatedInvoice == null)
                {
                    return NotFound("Invoice not found or validation failed.");
                }
                return Ok(updatedInvoice);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}