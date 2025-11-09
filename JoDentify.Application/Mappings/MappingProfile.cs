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

            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.JoinDate.ToString("o")));

            CreateMap<CreateUpdatePatientDto, Patient>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender));

            CreateMap<Patient, CreateUpdatePatientDto>();

            CreateMap<Patient, PatientDetailsDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.CurrentMedications, opt => opt.MapFrom(src => src.CurrentMedications))
                .ForMember(dest => dest.TreatmentPlan, opt => opt.MapFrom(src => src.TreatmentPlan))
                .ForMember(dest => dest.DentalHistory, opt => opt.MapFrom(src => src.DentalHistory))
                .ForMember(dest => dest.DentistNotes, opt => opt.MapFrom(src => src.DentistNotes))
                .ForMember(dest => dest.HeartRate, opt => opt.MapFrom(src => src.HeartRate))
                .ForMember(dest => dest.BloodPressure, opt => opt.MapFrom(src => src.BloodPressure))
                .ForMember(dest => dest.BloodSugar, opt => opt.MapFrom(src => src.BloodSugar))
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Temperature));


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
            CreateMap<PaymentTransaction, PaymentTransactionDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));

            CreateMap<CreateInvoiceDto, Invoice>();
            CreateMap<CreatePaymentDto, PaymentTransaction>();
        }
    }
}