using JoDentify.Application.DTOs.ClinicService;

namespace JoDentify.Application.Interfaces
{
    public interface IClinicServiceService
    {
        Task<ClinicServiceDto?> CreateServiceAsync(CreateUpdateClinicServiceDto createDto);
        Task<IEnumerable<ClinicServiceDto>> GetAllServicesForClinicAsync();
        Task<ClinicServiceDto?> GetServiceByIdAsync(Guid serviceId);
        Task<ClinicServiceDto?> UpdateServiceAsync(Guid serviceId, CreateUpdateClinicServiceDto updateDto);
        Task<bool> DeleteServiceAsync(Guid serviceId);
    }
}