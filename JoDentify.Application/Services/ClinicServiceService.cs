using AutoMapper;
using JoDentify.Application.DTOs.ClinicService;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JoDentify.Application.Services
{
    public class ClinicServiceService : IClinicServiceService
    {
        private readonly IRepository<ClinicService> _serviceRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public ClinicServiceService(
            IRepository<ClinicService> serviceRepository,
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceRepository = serviceRepository;
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

        public async Task<ClinicServiceDto?> CreateServiceAsync(CreateUpdateClinicServiceDto createDto)
        {
            CheckClinicAccess();

            var service = _mapper.Map<ClinicService>(createDto);
            service.ClinicId = _clinicId!.Value;

            await _serviceRepository.AddAsync(service);

            return _mapper.Map<ClinicServiceDto>(service);
        }

        public async Task<IEnumerable<ClinicServiceDto>> GetAllServicesForClinicAsync()
        {
            CheckClinicAccess();

            var services = await _context.ClinicServices
                                     .AsNoTracking()
                                     .Where(s => s.ClinicId == _clinicId!.Value)
                                     .ToListAsync();

            return _mapper.Map<IEnumerable<ClinicServiceDto>>(services);
        }

        public async Task<ClinicServiceDto?> GetServiceByIdAsync(Guid serviceId)
        {
            CheckClinicAccess();

            var service = await _context.ClinicServices
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s =>
                                        s.Id == serviceId &&
                                        s.ClinicId == _clinicId!.Value);

            return _mapper.Map<ClinicServiceDto?>(service);
        }

        public async Task<ClinicServiceDto?> UpdateServiceAsync(Guid serviceId, CreateUpdateClinicServiceDto updateDto)
        {
            CheckClinicAccess();

            var service = await _context.ClinicServices
                                    .FirstOrDefaultAsync(s =>
                                        s.Id == serviceId &&
                                        s.ClinicId == _clinicId!.Value);

            if (service == null)
            {
                return null;
            }

            _mapper.Map(updateDto, service);
            service.ClinicId = _clinicId!.Value;

            await _serviceRepository.UpdateAsync(service);

            return _mapper.Map<ClinicServiceDto>(service);
        }

        public async Task<bool> DeleteServiceAsync(Guid serviceId)
        {
            CheckClinicAccess();

            var service = await _context.ClinicServices
                                    .FirstOrDefaultAsync(s =>
                                        s.Id == serviceId &&
                                        s.ClinicId == _clinicId!.Value);

            if (service == null)
            {
                return false;
            }

            await _serviceRepository.DeleteAsync(service.Id);
            return true;
        }
    }
}