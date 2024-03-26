using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DAL
{
    public class HmoContext: DbContext
    {
        public HmoContext(DbContextOptions<HmoContext> options) : base(options)
        {            
        }

        public HmoContext()
        {                
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Illness> Illnesses { get; set; }
        public virtual DbSet<VaccinsManufacturer> VaccinsManufacturers { get; set; }
        public virtual DbSet<Vaccin> Vaccins { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);


        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
