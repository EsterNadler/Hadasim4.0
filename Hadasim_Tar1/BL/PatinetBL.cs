using Contracts;
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace BL
{
    public class PatinetBL
    {
        private readonly HmoContext context;

        public PatinetBL(HmoContext context)
        {
            this.context = context;
        }

        public async Task Create(PatientRequest patient)
        {
            using (var stream = new MemoryStream()) {

                if (patient.Image != null)
                    await patient.Image.CopyToAsync(stream);

                var dbPatient = new DAL.Models.Patient
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Phone = patient.Phone,
                    CellPhone = patient.CellPhone,
                    Address = patient.Address,
                    BirthDate = patient.BirthDate,
                    Image = patient.Image == null ? null : stream.ToArray()
                };
                try
                {
                    context.Patients.Add(dbPatient);
                    await context.SaveChangesAsync();
                }
                catch(DbUpdateException)
                {//TODO: log error
                    throw;
                }
            }
        }
        public async Task<IEnumerable<PatientResponse>> GetPatients()
        {
            try
            {
                var dbPatients = await context.Patients.ToListAsync();
                return dbPatients.Select(dbp => new PatientResponse
                {
                    Id = dbp.Id,
                    FirstName = dbp.FirstName,
                    LastName = dbp.LastName,
                    Phone = dbp.Phone,
                    CellPhone = dbp.CellPhone,
                    Address = dbp.Address,
                    BirthDate = dbp.BirthDate,
                    Image = dbp.Image
                });
            }
            catch (InvalidOperationException)
            {
                //TODO: log error
                throw;
            }
        }

        public async Task<PatientResponse> GetPatientById(string patientId)
        {
            var dbPatient = await context.Patients.FindAsync(patientId);
            if (dbPatient != null)
            {
                return new PatientResponse
                {
                    Id = dbPatient.Id,
                    FirstName = dbPatient.FirstName,
                    LastName = dbPatient.LastName,
                    Phone = dbPatient.Phone,
                    CellPhone = dbPatient.CellPhone,
                    Address = dbPatient.Address,
                    BirthDate = dbPatient.BirthDate,
                    Image = dbPatient.Image
                };
            }
            else
            {
                throw new KeyNotFoundException("Patient not found");
            }
        }

        public async Task Update(string id, PatientRequest patient)
        {
            var dbPatient = await context.Patients.FindAsync(id);
            
            if (dbPatient == null)
                throw new KeyNotFoundException();

            using (var stream = new MemoryStream())
            {
                if (patient.Image != null)
                    await patient.Image.CopyToAsync(stream);
                             
                dbPatient.FirstName = patient.FirstName;
                dbPatient.LastName = patient.LastName;
                dbPatient.Phone = patient.Phone;
                dbPatient.CellPhone = patient.CellPhone;
                dbPatient.Address = patient.Address;
                dbPatient.BirthDate = patient.BirthDate;
                if(patient.Image != null)
                    dbPatient.Image = stream.ToArray();

                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(string patientId)
        {
            var dbPatient = await context.Patients.FindAsync(patientId);
            if (dbPatient != null)
            {
                context.Patients.Remove(dbPatient);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Patient not found");
            }
        }
    }
}
