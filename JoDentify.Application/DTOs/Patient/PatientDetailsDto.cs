using JoDentify.Application.DTOs.Appointment;
using JoDentify.Application.DTOs.Billing;
using System.Collections.Generic;
using System;

namespace JoDentify.Application.DTOs.Patient
{
    public class PatientDetailsDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public string? CurrentMedications { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? DentalHistory { get; set; }
        public string? DentistNotes { get; set; }

        // Vitals
        public float? HeartRate { get; set; }
        public string? BloodPressure { get; set; }
        public float? BloodSugar { get; set; }
        public float? Temperature { get; set; }


        public ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
        public ICollection<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
    }
}