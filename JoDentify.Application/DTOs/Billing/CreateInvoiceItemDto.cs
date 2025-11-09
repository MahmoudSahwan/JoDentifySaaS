using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Billing
{
    public class CreateInvoiceItemDto
    {
        // --- (ده أهم تعديل) ---
        [Required(ErrorMessage = "ClinicServiceId is required.")]
        public Guid ClinicServiceId { get; set; } // <-- (الاسم ده لازم يطابق الفرونت إند)

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // السطر ده اختياري (زي ما هو عندك)
        public decimal? UnitPriceOverride { get; set; }
    }
}