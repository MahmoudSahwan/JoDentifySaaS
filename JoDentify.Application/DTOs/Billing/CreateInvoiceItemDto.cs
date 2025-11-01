using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Billing
{
    public class CreateInvoiceItemDto
    {
        [Required]
        public Guid ClinicServiceId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        [Range(0, 1_000_000, ErrorMessage = "Unit price must be a positive number.")]
        public decimal? UnitPriceOverride { get; set; }
    }
}