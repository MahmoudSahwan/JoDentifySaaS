using JoDentify.Application.DTOs.Patient;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;

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

    
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            
            var patients = await _patientService.GetAllPatientsForClinicAsync();
            return Ok(patients); 
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(); 
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var newPatient = await _patientService.CreatePatientAsync(createDto);

            return CreatedAtAction(nameof(GetPatientById), new { id = newPatient.Id }, newPatient);
        }

        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] CreatePatientDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPatient = await _patientService.UpdatePatientAsync(id, updateDto);

            if (updatedPatient == null)
            {
                return NotFound(); 
            }
            return Ok(updatedPatient);
        }

        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var result = await _patientService.DeletePatientAsync(id);

            if (!result)
            {
                return NotFound(); 
            }

            return NoContent();
        }
    }
}