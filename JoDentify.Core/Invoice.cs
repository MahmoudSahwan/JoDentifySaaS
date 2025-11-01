using JoDentify.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    public class Invoice : BaseEntity
    {
        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;

        [Required]
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        public Guid? AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 10_000_000)]
        public decimal TotalAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 10_000_000)]
        public decimal AmountPaid { get; set; } = 0;

        [NotMapped]
        public decimal AmountDue => TotalAmount - AmountPaid;

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        [Required]
        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;

        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    }
}