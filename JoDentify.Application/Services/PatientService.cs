using AutoMapper;
using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace JoDentify.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public PatientService(
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
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

        public async Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid id)
        {
            CheckClinicAccess();
            var clinicId = _clinicId.Value;

            var patient = await _context.Patients
                .AsNoTracking()
                .Include(p => p.Appointments.OrderByDescending(a => a.StartTime))
                    .ThenInclude(a => a.Doctor)
                .Include(p => p.Invoices.OrderByDescending(i => i.IssueDate))
                .FirstOrDefaultAsync(p => p.Id == id && p.ClinicId == clinicId);

            if (patient == null) return null;

            return _mapper.Map<PatientDetailsDto>(patient);
        }

        public async Task<PatientDto?> CreatePatientAsync(CreateUpdatePatientDto createDto)
        {
            CheckClinicAccess();
            var patient = _mapper.Map<Patient>(createDto);
            patient.ClinicId = _clinicId.Value;
            patient.JoinDate = DateTime.UtcNow;

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient); 
            patient.JoinDate = DateTime.UtcNow; 
        }

        public async Task<PatientDto?> UpdatePatientAsync(Guid id, CreateUpdatePatientDto updateDto)
        {
            CheckClinicAccess();
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && p.ClinicId == _clinicId.Value);

            if (patient == null) return null;

            _mapper.Map(updateDto, patient);
            await _context.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient); 
        }

        public async Task<CreateUpdatePatientDto?> GetPatientForEditAsync(Guid id)
        {
            CheckClinicAccess();
            var patient = await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.ClinicId == _clinicId.Value);

            return _mapper.Map<CreateUpdatePatientDto?>(patient);
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            CheckClinicAccess();
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && p.ClinicId == _clinicId.Value);

            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PatientDto?> GetPatientByIdAsync(Guid id)
        {
            CheckClinicAccess();
            var patient = await _context.Patients
               .AsNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id && p.ClinicId == _clinicId.Value);

            return _mapper.Map<PatientDto?>(patient);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            CheckClinicAccess();
            var patients = await _context.Patients
                .AsNoTracking()
                .Where(p => p.ClinicId == _clinicId.Value)
                .OrderByDescending(p => p.JoinDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
    }
}