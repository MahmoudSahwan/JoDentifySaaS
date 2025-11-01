using JoDentify.Application.DTOs.ClinicService;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoDentify.API.Controllers
{
    [Authorize]
    [Route("api/clinic-services")]
    [ApiController]
    public class ClinicServiceController : ControllerBase
    {
        private readonly IClinicServiceService _service;

        public ClinicServiceController(IClinicServiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var services = await _service.GetAllServicesForClinicAsync();
                return Ok(services);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            try
            {
                var service = await _service.GetServiceByIdAsync(id);
                if (service == null)
                {
                    return NotFound();
                }
                return Ok(service);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateUpdateClinicServiceDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newService = await _service.CreateServiceAsync(createDto);
                return CreatedAtAction(nameof(GetServiceById), new { id = newService.Id }, newService);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] CreateUpdateClinicServiceDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedService = await _service.UpdateServiceAsync(id, updateDto);
                if (updatedService == null)
                {
                    return NotFound();
                }
                return Ok(updatedService);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            try
            {
                var result = await _service.DeleteServiceAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}