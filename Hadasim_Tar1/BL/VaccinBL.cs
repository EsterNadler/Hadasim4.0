using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class VaccinBL
    {
        private readonly HmoContext context;

        public VaccinBL(HmoContext context)
        {
            this.context = context;
        }

        public async Task Create(Contracts.Vaccin vaccin)
        {
            var dbVaccin = new DAL.Models.Vaccin
            {
                Id = Guid.NewGuid(),//??????????????????????????????????????in contracts already...???
                Date= vaccin.Date,
                PatientID = vaccin.PatientID,
                VaccinsManufacturerID=vaccin.VaccinsManufacturerID,
                
            };
            context.Vaccins.Add(dbVaccin);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Contracts.Vaccin>> GetVaccins()
        {
            try
            {
                var dbvaccins = await context.Vaccins.ToListAsync();
                return dbvaccins.Select(dbv => new Contracts.Vaccin
                {
                    Id = dbv.Id,
                    Date = dbv.Date,
                    PatientID = dbv.PatientID,
                    VaccinsManufacturerID = dbv.VaccinsManufacturerID,
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Contracts.Vaccin> GetVaccinById(Guid vaccinId)
        {
            var dbVaccin = await context.Vaccins.FindAsync(vaccinId);
            if (dbVaccin != null)
            {
                return new Contracts.Vaccin
                {
                    Id = dbVaccin.Id,
                    Date = dbVaccin.Date,
                    PatientID = dbVaccin.PatientID,
                    VaccinsManufacturerID = dbVaccin.VaccinsManufacturerID,
                };
            }
            else
            {
                throw new Exception("Vaccin not found");
            }
        }
        public async Task Update(Contracts.Vaccin vaccin)
        {
            var dbVaccin = await context.Vaccins.FindAsync(vaccin.Id);
            if (dbVaccin != null)
            {
                //dbIllness.Id = illness.Id;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                dbVaccin.Date = vaccin.Date;
                dbVaccin.PatientID = vaccin.PatientID;
                dbVaccin.VaccinsManufacturerID = vaccin.VaccinsManufacturerID;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Vaccin not found");
            }
        }

        public async Task Delete(Guid vaccinId)
        {
            var dbVaccin = await context.Vaccins.FindAsync(vaccinId);
            if (vaccinId != null)
            {
                context.Vaccins.Remove(dbVaccin);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Vaccin not found");
            }
        }
    }
}
