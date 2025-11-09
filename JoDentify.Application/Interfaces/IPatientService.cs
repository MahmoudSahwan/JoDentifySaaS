using JoDentify.Application.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoDentify.Application.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto?> CreatePatientAsync(CreateUpdatePatientDto createDto);
        Task<PatientDto?> UpdatePatientAsync(Guid id, CreateUpdatePatientDto updateDto);

        Task<bool> DeletePatientAsync(Guid id);

        Task<CreateUpdatePatientDto?> GetPatientForEditAsync(Guid id);

        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid id);
    }
}