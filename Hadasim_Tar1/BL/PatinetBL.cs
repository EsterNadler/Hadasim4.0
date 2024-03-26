using Contracts;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class PatinetBL
    {
        private readonly HmoContext context;

        public PatinetBL(HmoContext context)
        {
            this.context = context;
        }

        public async Task Create(Patient patient)
        {
            var dbPatient = new DAL.Models.Patient
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Phone = patient.Phone,
                CellPhone = patient.CellPhone,
                Address = patient.Address,
                BirthDate = patient.BirthDate
            };
            context.Patients.Add(dbPatient);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            try
            {
                var dbPatients = await context.Patients.ToListAsync();
                return dbPatients.Select(dbp => new Patient
                {
                    Id = dbp.Id,
                    FirstName = dbp.FirstName,
                    LastName = dbp.LastName,
                    Phone = dbp.Phone,
                    CellPhone = dbp.CellPhone,
                    Address = dbp.Address,
                    BirthDate = dbp.BirthDate
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Patient> GetPatientById(string patientId)
        {
            var dbPatient = await context.Patients.FindAsync(patientId);
            if (dbPatient != null)
            {
                return new Patient
                {
                    Id = dbPatient.Id,
                    FirstName = dbPatient.FirstName,
                    LastName = dbPatient.LastName,
                    Phone = dbPatient.Phone,
                    CellPhone = dbPatient.CellPhone,
                    Address = dbPatient.Address,
                    BirthDate = dbPatient.BirthDate
                };
            }
            else
            {
                throw new Exception("Patient not found");
            }
        }

        public async Task Update(string id, Patient patient)
        {
            var dbPatient = await context.Patients.FindAsync(id);
            
            if (dbPatient == null)
                throw new KeyNotFoundException();

            dbPatient.FirstName = patient.FirstName;
            dbPatient.LastName = patient.LastName;
            dbPatient.Phone = patient.Phone;
            dbPatient.CellPhone = patient.CellPhone;
            dbPatient.Address = patient.Address;
            dbPatient.BirthDate = patient.BirthDate;
            await context.SaveChangesAsync();
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
