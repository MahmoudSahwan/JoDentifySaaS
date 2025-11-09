using JoDentify.Application.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoDentify.Application.Interfaces
{
    public interface IPatientService
    {
        // (تعديل)
        Task<PatientDto?> CreatePatientAsync(CreateUpdatePatientDto createDto);
        // (تعديل)
        Task<PatientDto?> UpdatePatientAsync(Guid id, CreateUpdatePatientDto updateDto);

        Task<bool> DeletePatientAsync(Guid id);

        // (تعديل) الدالة دي هترجع الفورم الجديد عشان "التعديل"
        Task<CreateUpdatePatientDto?> GetPatientForEditAsync(Guid id);

        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        // (جديد)
        Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid id);
    }
}