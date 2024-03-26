using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class VaccinsManufacturerBL
    {
        private readonly HmoContext context;

        public VaccinsManufacturerBL(HmoContext context)
        {
            this.context = context;
        }

        public async Task Create(Contracts.VaccinsManufacturer vm)
        {
            var dbvm = new DAL.Models.VaccinsManufacturer
            {
                Id = Guid.NewGuid(),//??????????????????????????????????????in contracts already...???
                Name = vm.Name

            };
            context.VaccinsManufacturers.Add(dbvm);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Contracts.VaccinsManufacturer>> GetVaccinsManufacturer()
        {
            try
            {
                var dbvm = await context.VaccinsManufacturers.ToListAsync();
                return dbvm.Select(dbvm => new Contracts.VaccinsManufacturer
                {
                    Id = dbvm.Id,
                  Name = dbvm.Name
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Contracts.VaccinsManufacturer> GetVaccinsManufacturerById(Guid vaccinsManufacturerId)
        {
            var dbvm = await context.VaccinsManufacturers.FindAsync(vaccinsManufacturerId);
            if (dbvm != null)
            {
                return new Contracts.VaccinsManufacturer
                {
                    Id = dbvm.Id,
                    Name= dbvm.Name
                };
            }
            else
            {
                throw new Exception("VaccinsManufacturer not found");
            }
        }
        public async Task Update(Contracts.VaccinsManufacturer vm)
        {
            var dbvm = await context.VaccinsManufacturers.FindAsync(vm.Id);
            if (dbvm != null)
            {
                //dbIllness.Id = illness.Id;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
               dbvm.Name = vm.Name;
            }
            else
            {
                throw new Exception("VaccinsManufacturer not found");
            }
        }

        public async Task Delete(Guid vmId)
        {
            var dbvm = await context.VaccinsManufacturers.FindAsync(vmId);
            if (vmId != null)
            {
                context.VaccinsManufacturers.Remove(dbvm);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("VaccinsManufacturer not found");
            }
        }
    }
}
