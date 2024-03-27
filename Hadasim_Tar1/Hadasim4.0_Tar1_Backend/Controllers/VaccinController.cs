using BL;
using Contracts;
using Hadasim4._0_Tar1_Backend.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hadasim4._0_Tar1_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ValidateModelState]
    public class VaccinController:ControllerBase
    {
        private readonly VaccinBL bl;

        public VaccinController(VaccinBL bl)
        {
            this.bl = bl;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //return await bl.GetPatientById(id);
            var vaccin = await bl.GetVaccinById(id);
            if (vaccin == null)
            {
                return NotFound();
            }
            return Ok(vaccin);
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            //return await bl.GetPatientById(id);
            var vaccin = await bl.GetVaccinsByPatientID(id);
            if (vaccin == null)
            {
                return NotFound();
            }
            return Ok(vaccin);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vaccins = await bl.GetVaccins();
            if (vaccins == null || !vaccins.Any())
            {
                return NotFound("No vaccins found");
            }
            return Ok(vaccins);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vaccin vaccin)//from body json////
        {
            if (vaccin == null)
            {
                return BadRequest("Vaccin object is null");
            }

            await bl.Create(vaccin);
            return CreatedAtAction(nameof(Get), new { id = vaccin.Id }, vaccin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Vaccin vaccin)
        {
            if (vaccin == null)
            {
                return BadRequest();
            }

            try
            {
                await bl.GetVaccinById(id);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await bl.Delete(id);
            return NoContent();
        }
    }
}
