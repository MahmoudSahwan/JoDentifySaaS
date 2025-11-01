using JoDentify.Application.DTOs.Patient;

namespace JoDentify.Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsForClinicAsync();

        Task<PatientDto?> GetPatientByIdAsync(Guid patientId);

        Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto);

        Task<PatientDto?> UpdatePatientAsync(Guid patientId, CreatePatientDto updateDto);

        Task<bool> DeletePatientAsync(Guid patientId);
    }
}