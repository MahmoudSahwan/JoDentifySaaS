using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class InvoiceItem : BaseEntity
    {
        [Required]
        public Guid InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; } = null!;

        [Required]
        public Guid ClinicServiceId { get; set; }
        [ForeignKey("ClinicServiceId")]
        public ClinicService ClinicService { get; set; } = null!;

        [Required(ErrorMessage = "Item Name is required.")]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 1_000_000)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 10_000_000)]
        public decimal TotalPrice { get; set; }

        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;
    }
}