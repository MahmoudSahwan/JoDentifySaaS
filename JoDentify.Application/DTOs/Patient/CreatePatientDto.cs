using JoDentify.Core;
using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Patient
{
    public class CreateUpdatePatientDto
    {
        // ===============================
        // 🧍 PERSONAL INFORMATION
        // ===============================
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(20)]
        public string? SecondaryPhone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        public string NationalId { get; set; } = string.Empty;

        [StringLength(100)]
        public string Occupation { get; set; } = string.Empty;

        [StringLength(50)]
        public string MaritalStatus { get; set; } = string.Empty;

        [StringLength(50)]
        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        // ===============================
        // 🏥 EMERGENCY CONTACT
        // ===============================
        [StringLength(100)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [StringLength(20)]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // ===============================
        // ⚕️ MEDICAL & DENTAL BACKGROUND
        // ===============================
        public string MedicalHistory { get; set; } = string.Empty;
        public string ChronicDiseases { get; set; } = string.Empty;
        public string CurrentMedications { get; set; } = string.Empty;

        public string DentalHistory { get; set; } = string.Empty;
        public bool IsSmoker { get; set; }
        public bool HasBraces { get; set; }

        public string OralCareRoutine { get; set; } = string.Empty;
        public string LastVisitReason { get; set; } = string.Empty;
        public DateTime? LastVisitDate { get; set; }

        public string PreferredDoctor { get; set; } = string.Empty;
        public string DentistNotes { get; set; } = string.Empty;

        // ===============================
        // 📊 VITALS & MEASUREMENTS
        // ===============================
        public float? HeartRate { get; set; }
        public string? BloodPressure { get; set; } // e.g., "120/80"
        public float? BloodSugar { get; set; }
        public float? Temperature { get; set; }
        public float? OxygenLevel { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }

        // ===============================
        // 🦷 DENTAL PLAN
        // ===============================
        public string DentalInsuranceType { get; set; } = string.Empty;
        public DateTime? LastXRayDate { get; set; }
        public string TreatmentPlan { get; set; } = string.Empty;

        // ===============================
        // 💳 INSURANCE & MEMBERSHIP
        // ===============================
        [StringLength(200)]
        public string InsuranceProvider { get; set; } = string.Empty;

        [StringLength(100)]
        public string InsuranceNumber { get; set; } = string.Empty;

        public DateTime? InsuranceExpiryDate { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; } = string.Empty;

        [StringLength(50)]
        public string MembershipStatus { get; set; } = string.Empty;

        // ===============================
        // 💬 NOTES & COMMUNICATION
        // ===============================
        public string NotesFromDoctor { get; set; } = string.Empty;
        public string PatientFeedback { get; set; } = string.Empty;

        public bool IsReferredByFriend { get; set; }
        public string ReferralSource { get; set; } = string.Empty;
    }
}