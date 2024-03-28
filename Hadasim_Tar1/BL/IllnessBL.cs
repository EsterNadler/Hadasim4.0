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

        //ctor
        public IllnessBL(HmoContext context)
        {
            this.context = context;
        }

        //adding new illness
        public async Task Create(Contracts.Illness illness)
        {
            var dbillness = new DAL.Models.Illness
            {
                Id = Guid.NewGuid(),
                PositiveDate = illness.PositiveDate,
                NegativeDate = illness.NegativeDate,
                PatientId = illness.PatientId
            };
            try
            {
                context.Illnesses.Add(dbillness);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {//TODO: log error
                throw;
            }
        }

        //get all/part of illnesses
        public async Task<IEnumerable<Contracts.Illness>> GetIllnessess(Func<DAL.Models.Illness, bool>? func = null)
        {
            List<DAL.Models.Illness> dbillnesses;

            try
            {
                if (func == null)
                    dbillnesses = await context.Illnesses.ToListAsync();
                else
                    dbillnesses = await context.Illnesses./*Where(func).*/ToListAsync();//Problem running ToListAsync on IEnumerable

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
                //TODO: log error
                throw;
            }
        }

        //get illnesses of certain patient
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
                }).Where(dbp => dbp.PatientId == patient);
            }
            catch (InvalidOperationException)
            {//TODO: log error
                throw;
            }
        }

        //get illness by id
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

        //update illness
        public async Task Update(Contracts.Illness illness)
        {
            var dbIllness = await context.Illnesses.FindAsync(illness.Id);
            if (dbIllness != null)
            {
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

        //delete illness
        public async Task Delete(Guid illnessId)
        {
            var dbIllness = await context.Illnesses.FindAsync(illnessId);
            if (illnessId != null)
            {
                context.Illnesses.Remove(dbIllness!);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Illness not found");
            }
        }


        public async Task<object> GetStats()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            Random random = new Random();
            DateTime endDate = DateTime.Today.Date;
            DateTime startDate = endDate.AddDays(-29).Date;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string key = date.ToString("dd/MM/yyyy");
                dictionary.Add(key, 0);
            }
            Func<DAL.Models.Illness, bool> inDateRange = (illness) => illness.NegativeDate.Date > DateTime.Today.AddDays(-30) || illness.PositiveDate.Date > DateTime.Today.AddDays(-30);
            var illnesses = GetIllnessess(inDateRange);
            foreach (var illness in await illnesses)
            {
                for (var i = illness.PositiveDate.Date; i <= illness.NegativeDate.Date; i = i.AddDays(1))
                {
                    if (i >= startDate && i <= endDate && (i<illness.NegativeDate||illness.NegativeDate==illness.PositiveDate))
                        dictionary[i.ToString("dd/MM/yyyy")]++;
                }
            }
            return dictionary;
        }

    }
}
