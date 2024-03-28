﻿using Contracts;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                Id = Guid.NewGuid(),
                Date = vaccin.Date,
                PatientID = vaccin.PatientID,
                VaccinsManufacturerID = vaccin.VaccinsManufacturerID,

            };
            try
            {
                context.Vaccins.Add(dbVaccin);
                await context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                //TODO: log error
                throw;
            }
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
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Contracts.Vaccin>> GetVaccinsByPatientID(string patient)
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
                }).Where(dbv => dbv.PatientID == patient);
            }
            catch (InvalidOperationException)
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

        public async Task<int> GetVaccinatedCount()
        {
            return (await GetVaccins()).GroupBy(x => x.PatientID).Count();
        }

        public async Task<int> GetUnvaccinatedCount()
        {
            return await context.Patients.CountAsync() - await GetVaccinatedCount();
        }
    }
}
