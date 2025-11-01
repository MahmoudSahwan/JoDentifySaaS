using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class PaymentTransaction : BaseEntity
    {
        [Required]
        public Guid InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; } = null!;

        [Required(ErrorMessage = "Payment amount is required.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 10_000_000, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Payment method is required.")]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;
    }
}