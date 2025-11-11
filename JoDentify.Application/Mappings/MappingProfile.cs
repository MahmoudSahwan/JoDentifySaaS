using AutoMapper;
using JoDentify.Application.DTOs.Auth;
using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.DTOs.Appointment;
using JoDentify.Application.DTOs.User;
using JoDentify.Application.DTOs.ClinicService;
using JoDentify.Application.DTOs.Billing;
using JoDentify.Core;

namespace JoDentify.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // --- (إصلاحات الـ Patient) ---

            // (1) DTO الجدول (خفيف)
            CreateMap<Patient, PatientDto>()
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.JoinDate.ToString("o")));
            // (تم حذف الـ 8 أسطر الخاطئة من هنا)

            CreateMap<CreateUpdatePatientDto, Patient>()
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender));

            CreateMap<Patient, CreateUpdatePatientDto>()
                      // (إضافة) تحويل التواريخ لـ string للفورم
                      .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString("yyyy-MM-dd") : null))
                      .ForMember(dest => dest.LastVisitDate, opt => opt.MapFrom(src => src.LastVisitDate.HasValue ? src.LastVisitDate.Value.ToString("yyyy-MM-dd") : null))
                      .ForMember(dest => dest.LastXRayDate, opt => opt.MapFrom(src => src.LastXRayDate.HasValue ? src.LastXRayDate.Value.ToString("yyyy-MM-dd") : null))
                      .ForMember(dest => dest.InsuranceExpiryDate, opt => opt.MapFrom(src => src.InsuranceExpiryDate.HasValue ? src.InsuranceExpiryDate.Value.ToString("yyyy-MM-dd") : null));


            // (2) DTO التفاصيل (ثقيل)
            CreateMap<Patient, PatientDetailsDto>()
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.JoinDate.Date))
                // --- (تم نقل الـ 8 أسطر هنا) ---
                .ForMember(dest => dest.CurrentMedications, opt => opt.MapFrom(src => src.CurrentMedications))
                .ForMember(dest => dest.TreatmentPlan, opt => opt.MapFrom(src => src.TreatmentPlan))
                .ForMember(dest => dest.DentalHistory, opt => opt.MapFrom(src => src.DentalHistory))
                .ForMember(dest => dest.DentistNotes, opt => opt.MapFrom(src => src.DentistNotes))
                .ForMember(dest => dest.HeartRate, opt => opt.MapFrom(src => src.HeartRate))
                .ForMember(dest => dest.BloodPressure, opt => opt.MapFrom(src => src.BloodPressure))
                .ForMember(dest => dest.BloodSugar, opt => opt.MapFrom(src => src.BloodSugar))
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Temperature));
            // --- (نهاية التعديلات) ---

            // --- (تعديلات الفواتير) ---
            CreateMap<Appointment, AppointmentDto>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
        .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
        .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<ClinicService, ClinicServiceDto>()
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateUpdateClinicServiceDto, ClinicService>();

            CreateMap<Invoice, InvoiceDto>()
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
              .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<InvoiceItem, InvoiceItemDto>();
            CreateMap<PaymentTransaction, PaymentTransactionDto>(); // (إضافة)
            CreateMap<CreatePaymentDto, PaymentTransaction>(); // (إضافة)

            CreateMap<CreateInvoiceDto, Invoice>()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate.ToUniversalTime()))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.ToUniversalTime() : (DateTime?)null));
            CreateMap<CreatePaymentDto, PaymentTransaction>()
                      .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.ToUniversalTime()));
        }
    }
}