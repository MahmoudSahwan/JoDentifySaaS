using AutoMapper;
using JoDentify.Application.DTOs.Auth;
using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.DTOs.Appointment;
using JoDentify.Application.DTOs.ClinicService;
using JoDentify.Application.DTOs.Billing;
using JoDentify.Application.DTOs.Dashboard;
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
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
            CreateMap<CreatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<ClinicService, ClinicServiceDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateUpdateClinicServiceDto, ClinicService>();

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));
            CreateMap<InvoiceItem, InvoiceItemDto>();
            CreateMap<PaymentTransaction, PaymentTransactionDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));

            CreateMap<CreateInvoiceItemDto, InvoiceItem>();
            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<Patient, RecentPatientDto>();
        }
    }
}