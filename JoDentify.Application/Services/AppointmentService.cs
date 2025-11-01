using AutoMapper;
using JoDentify.Application.DTOs.Appointment;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JoDentify.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
            _mapper = mapper;

            var user = httpContextAccessor.HttpContext?.User;
            var clinicIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "clinicId");
            if (clinicIdClaim != null && Guid.TryParse(clinicIdClaim.Value, out Guid clinicId))
            {
                _clinicId = clinicId;
            }
            else
            {
                _clinicId = null;
            }
        }

        private void CheckClinicAccess()
        {
            if (_clinicId == null)
            {
                throw new UnauthorizedAccessException("User is not associated with a clinic.");
            }
        }

        private async Task<AppointmentDto?> GetFullAppointmentDto(Guid appointmentId)
        {
            var appointment = await _context.Appointments
               .AsNoTracking()
               .Include(a => a.Patient)
               .Include(a => a.Doctor)
               .FirstOrDefaultAsync(a => a.Id == appointmentId && a.ClinicId == _clinicId.Value);

            return _mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<AppointmentDto?> CreateAppointmentAsync(CreateAppointmentDto createDto)
        {
            CheckClinicAccess();

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == createDto.PatientId && p.ClinicId == _clinicId.Value);
            var doctorExists = await _context.Users.AnyAsync(u => u.Id == createDto.DoctorId && u.ClinicId == _clinicId.Value);

            if (!patientExists || !doctorExists)
            {
                return null;
            }

            var appointment = _mapper.Map<Appointment>(createDto);
            appointment.ClinicId = _clinicId.Value;

            await _appointmentRepository.AddAsync(appointment);

            return await GetFullAppointmentDto(appointment.Id);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsForClinicAsync()
        {
            CheckClinicAccess();

            var appointments = await _context.Appointments
                                     .AsNoTracking()
                                     .Where(a => a.ClinicId == _clinicId.Value)
                                     .Include(a => a.Patient)
                                     .Include(a => a.Doctor)
                                     .OrderBy(a => a.StartTime)
                                     .ToListAsync();

            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            CheckClinicAccess();
            return await GetFullAppointmentDto(appointmentId);
        }

        public async Task<AppointmentDto?> UpdateAppointmentAsync(Guid appointmentId, CreateAppointmentDto updateDto)
        {
            CheckClinicAccess();

            var appointment = await _context.Appointments
                                    .FirstOrDefaultAsync(a =>
                                        a.Id == appointmentId &&
                                        a.ClinicId == _clinicId.Value);

            if (appointment == null)
            {
                return null;
            }

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == updateDto.PatientId && p.ClinicId == _clinicId.Value);
            var doctorExists = await _context.Users.AnyAsync(u => u.Id == updateDto.DoctorId && u.ClinicId == _clinicId.Value);

            if (!patientExists || !doctorExists)
            {
                return null;
            }

            _mapper.Map(updateDto, appointment);
            appointment.ClinicId = _clinicId.Value;

            await _appointmentRepository.UpdateAsync(appointment);

            return await GetFullAppointmentDto(appointment.Id);
        }

        public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            CheckClinicAccess();

            var appointment = await _context.Appointments
                                    .FirstOrDefaultAsync(a =>
                                        a.Id == appointmentId &&
                                        a.ClinicId == _clinicId.Value);

            if (appointment == null)
            {
                return false;
            }

            await _appointmentRepository.DeleteAsync(appointment.Id);
            return true;
        }
    }
}