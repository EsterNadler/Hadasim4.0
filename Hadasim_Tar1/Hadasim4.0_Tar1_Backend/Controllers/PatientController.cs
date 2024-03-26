using BL;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hadasim4._0_Tar1_Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
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
            //return await bl.GetPatientById(id);
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
        public async Task<IActionResult> Create([FromBody] Patient patient)//from body json////
        {
            if (patient == null)
            {
                return BadRequest("Patient object is null");
            }

            await bl.Create(patient);
            return CreatedAtAction(nameof(Get), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Patient patient)
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
            //var existingPatient = await bl.GetPatientById(id);
            //if (existingPatient == null)
            //{
            //    return NotFound();
            //}

            //await bl.Update(patient);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //var patientToDelete = await bl.GetPatientById(id);
            //if (patientToDelete == null)
            //{
            //    return NotFound();
            //}

            if (id == null)
                return BadRequest();

            await bl.Delete(id);
            return NoContent();
        }

    }
}