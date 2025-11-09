using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JoDentify.API.Controllers
{
    [Authorize]
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // --- (جديد) ---
        [HttpGet("{id:guid}/details")]
        public async Task<IActionResult> GetPatientDetails(Guid id)
        {
            try
            {
                var patientDetails = await _patientService.GetPatientDetailsAsync(id);
                if (patientDetails == null)
                {
                    return NotFound();
                }
                return Ok(patientDetails);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        // --- (نهاية الإضافة) ---

        // --- (تعديل) ---
        // (الدالة دي دلوقتي بترجع الفورم التقيل عشان التعديل)
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatientForEdit(Guid id)
        {
            try
            {
                var patientDto = await _patientService.GetPatientForEditAsync(id);
                if (patientDto == null)
                {
                    return NotFound();
                }
                return Ok(patientDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        // --- (نهاية التعديل) ---

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientService.GetAllPatientsAsync();
                return Ok(patients);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // --- (تعديل) ---
        // (بيستقبل DTO الجديد)
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreateUpdatePatientDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patientDto = await _patientService.CreatePatientAsync(createDto);
                // (بيرجع DTO خفيف)
                return CreatedAtAction(nameof(GetPatientDetails), new { id = patientDto.Id }, patientDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // --- (تعديل) ---
        // (بيستقبل DTO الجديد)
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] CreateUpdatePatientDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patientDto = await _patientService.UpdatePatientAsync(id, updateDto);
                if (patientDto == null)
                {
                    return NotFound();
                }
                return Ok(patientDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        // --- (نهاية التعديل) ---

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                var result = await _patientService.DeletePatientAsync(id);
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