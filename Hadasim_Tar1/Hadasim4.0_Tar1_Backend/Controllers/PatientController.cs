using BL;
using Contracts;
using Hadasim4._0_Tar1_Backend.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hadasim4._0_Tar1_Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [ValidateModelState]
    public class PatientController : ControllerBase
    {
        private readonly PatinetBL bl;

        public PatientController(PatinetBL bl)
        {
            this.bl = bl;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var patient = await bl.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var patients = await bl.GetPatients();
            if (patients == null || !patients.Any())
            {
                return NotFound("No patients found");
            }
            return Ok(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PatientRequest patient)
        {
            try
            {
                if (patient == null)
                {
                    return BadRequest("Patient object is null");
                }

                await bl.Create(patient);
                return CreatedAtAction(nameof(Get), new { id = patient.Id }, patient);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] PatientRequest patient)
        {
            if (id == null || patient == null)
            {
                return BadRequest();
            }

            try
            {
                await bl.Update(id, patient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null)
                return BadRequest();

            await bl.Delete(id);
            return NoContent();
        }

    }
}