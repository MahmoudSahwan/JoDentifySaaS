using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoDentify.Core
{
    // (ده فصلناه في ملف لوحده عشان ننظم الكود)
    public class PatientFile : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        [StringLength(50)]
        public string FileType { get; set; } = string.Empty; // X-Ray, Report, Lab, etc.

        [Required]
        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; } = null!;

        [Required]
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;
    }
}