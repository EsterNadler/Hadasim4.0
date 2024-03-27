using BL;
using Contracts;
using Hadasim4._0_Tar1_Backend.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hadasim4._0_Tar1_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ValidateModelState]
    public class IllnessController:ControllerBase
    {
        private readonly IllnessBL bl;

        public IllnessController(IllnessBL bl)
        {
            this.bl = bl;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //return await bl.GetPatientById(id);
            var illness = await bl.GetillnessById(id);
            if (illness == null)
            {
                return NotFound();
            }
            return Ok(illness);
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            //return await bl.GetPatientById(id);
            var illness = await bl.GetillnessesByPatientId(id);
            if (illness == null)
            {
                return NotFound();
            }
            return Ok(illness);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var illnesses = await bl.GetIllnessess();
            if (illnesses == null || !illnesses.Any())
            {
                return NotFound("No illnesses found");
            }
            return Ok(illnesses);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Illness illness)//from body json////
        {
            if (illness == null)
            {
                return BadRequest("Illness object is null");
            }

            await bl.Create(illness);
            return CreatedAtAction(nameof(Get), new { id = illness.Id }, illness);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Illness illness)
        {
            if (illness == null)
            {
                return BadRequest();
            }

            try
            {
                await bl.GetillnessById(id);
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
