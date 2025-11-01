using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class ClinicService : BaseEntity
    {
        [Required(ErrorMessage = "Service name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Service name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 1_000_000, ErrorMessage = "Price must be a positive number.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(ServiceStatus), ErrorMessage = "Invalid status value.")]
        public ServiceStatus Status { get; set; } = ServiceStatus.Active;

        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;

        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}