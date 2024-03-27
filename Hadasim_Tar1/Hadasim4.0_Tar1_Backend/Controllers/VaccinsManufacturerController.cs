using BL;
using Contracts;
using DAL.Models;
using Hadasim4._0_Tar1_Backend.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hadasim4._0_Tar1_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ValidateModelState]
    public class VaccinsManufacturerController:ControllerBase
    {
        private readonly VaccinsManufacturerBL bl;

        public VaccinsManufacturerController(VaccinsManufacturerBL bl)
        {
            this.bl = bl;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //return await bl.GetPatientById(id);
            var vm = await bl.GetVaccinsManufacturerById(id);
            if (vm == null)
            {
                return NotFound();
            }
            return Ok(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vms = await bl.GetVaccinsManufacturer();
            if (vms == null || !vms.Any())
            {
                return NotFound("No vaccins manufacturers found");
            }
            return Ok(vms);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contracts.VaccinsManufacturer vm)//from body json////
        {
            if (vm == null)
            {
                return BadRequest("Vaccin object is null");
            }

            await bl.Create(vm);
            return CreatedAtAction(nameof(Get), new { id = vm.Id }, vm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Contracts.VaccinsManufacturer vm)
        {
            if (vm == null)
            {
                return BadRequest();
            }

            try
            {
                await bl.GetVaccinsManufacturerById(id);
            }
            catch(KeyNotFoundException)
            {
                NotFound();
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
