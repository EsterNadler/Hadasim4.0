using Contracts;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class IllnessBL
    {
        private readonly HmoContext context;

        public IllnessBL(HmoContext context)
        {
            this.context = context;
        }

        public async Task Create(Contracts.Illness illness)
        {
            var dbillness = new DAL.Models.Illness
            {
                Id = Guid.NewGuid(),
                PositiveDate = illness.PositiveDate,
                NegativeDate = illness.NegativeDate,
                PatientId=illness.PatientId
            };
            try
            {
                context.Illnesses.Add(dbillness);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Contracts.Illness>> GetIllnessess()
        {
            try
            {
                var dbillnesses = await context.Illnesses.ToListAsync();
                return dbillnesses.Select(dbi => new Contracts.Illness
                {
                    Id = dbi.Id,
                    PositiveDate = dbi.PositiveDate,
                    NegativeDate = dbi.NegativeDate,
                    PatientId = dbi.PatientId
                });
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Contracts.Illness>> GetillnessesByPatientId(string patient)
        {
            try
            {
                var dbillnesses = await context.Illnesses.ToListAsync();
                return dbillnesses.Select(dbi => new Contracts.Illness
                {
                    Id = dbi.Id,
                    PositiveDate = dbi.PositiveDate,
                    NegativeDate = dbi.NegativeDate,
                    PatientId = dbi.PatientId
                }).Where(dbp=>dbp.PatientId==patient);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Contracts.Illness> GetillnessById(Guid illnessId)
        {
            var dbIllness = await context.Illnesses.FindAsync(illnessId);
            if (dbIllness != null)
            {
                return new Contracts.Illness
                {
                    Id = dbIllness.Id,
                    PositiveDate = dbIllness.PositiveDate,
                    NegativeDate = dbIllness.NegativeDate,
                    PatientId = dbIllness.PatientId
                };
            }
            else
            {
                throw new Exception("Illness not found");
            }
        }
        public async Task Update(Contracts.Illness illness)
        {
            var dbIllness = await context.Illnesses.FindAsync(illness.Id);
            if (dbIllness != null)
            {
                //dbIllness.Id = illness.Id;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                dbIllness.PositiveDate = illness.PositiveDate;
                dbIllness.NegativeDate = illness.NegativeDate;
                dbIllness.PatientId = illness.PatientId;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Patient not found");
            }
        }

        public async Task Delete(Guid illnessId)
        {
            var dbIllness = await context.Illnesses.FindAsync(illnessId);
            if (illnessId != null)
            {
                context.Illnesses.Remove(dbIllness);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Illness not found");
            }
        }

        public object GetStats()
        {
            throw new NotImplementedException();
        }
    }
}
