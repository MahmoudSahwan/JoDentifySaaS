using JoDentify.Application.DTOs.Appointment;

namespace JoDentify.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> CreateAppointmentAsync(CreateAppointmentDto createDto);
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsForClinicAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId);
        Task<AppointmentDto?> UpdateAppointmentAsync(Guid appointmentId, CreateAppointmentDto updateDto);
        Task<bool> DeleteAppointmentAsync(Guid appointmentId);
    }
}