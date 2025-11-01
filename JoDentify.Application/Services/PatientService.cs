using AutoMapper;
using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data; 
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore; 
using System.Security.Claims; 

namespace JoDentify.Application.Services
{
 
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository; 
        private readonly ApplicationDbContext _context; 
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientService(
            IRepository<Patient> patientRepository,
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _patientRepository = patientRepository;
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid? GetClinicIdFromToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return null;
            }

            var clinicIdClaim = user.Claims.FirstOrDefault(c => c.Type == "clinicId");

            if (clinicIdClaim == null)
            {
                return null;
            }

            if (Guid.TryParse(clinicIdClaim.Value, out Guid clinicId))
            {
                return clinicId;
            }

            return null;
        }


        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto)
        {
            var clinicId = GetClinicIdFromToken();
            if (clinicId == null)
            {
                throw new UnauthorizedAccessException("User is not associated with a clinic.");
            }

            var patient = _mapper.Map<Patient>(createDto);

            patient.ClinicId = clinicId.Value;

            await _patientRepository.AddAsync(patient);

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsForClinicAsync()
        {

            var clinicId = GetClinicIdFromToken();
            if (clinicId == null)
            {
      
                return new List<PatientDto>();
            }


            var patients = await _context.Patients
                                     .Where(p => p.ClinicId == clinicId.Value)
                                     .ToListAsync();

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(Guid patientId)
        {
            var clinicId = GetClinicIdFromToken();
            if (clinicId == null) return null;

            var patient = await _context.Patients
                                    .FirstOrDefaultAsync(p =>
                                        p.Id == patientId &&
                                        p.ClinicId == clinicId.Value);

            return _mapper.Map<PatientDto?>(patient);
        }

        public async Task<PatientDto?> UpdatePatientAsync(Guid patientId, CreatePatientDto updateDto)
        {
            var clinicId = GetClinicIdFromToken();
            if (clinicId == null) return null;

            var patient = await _context.Patients
                                    .FirstOrDefaultAsync(p =>
                                        p.Id == patientId &&
                                        p.ClinicId == clinicId.Value);

            if (patient == null)
            {
                return null;
            }

            _mapper.Map(updateDto, patient);

            await _patientRepository.UpdateAsync(patient);

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> DeletePatientAsync(Guid patientId)
        {
            var clinicId = GetClinicIdFromToken();
            if (clinicId == null) return false;

            var patient = await _context.Patients
                                    .FirstOrDefaultAsync(p =>
                                        p.Id == patientId &&
                                        p.ClinicId == clinicId.Value);

            if (patient == null)
            {
                return false;
            }

            await _patientRepository.DeleteAsync(patient.Id);
            return true;
        }
    }
}