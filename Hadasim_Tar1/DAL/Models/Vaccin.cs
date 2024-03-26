using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Vaccin//public for access from other projects
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [ForeignKey(nameof(VaccinsManufacturer))]
        public Guid VaccinsManufacturerID { get; set; }
        public VaccinsManufacturer vaccinsManufacturer { get; set; } = default!;

        [Required]
        [ForeignKey(nameof(Patient))]
        public string PatientID { get; set; }= default!;
        public Patient Patient { get; set; } = default!;


    }
}
