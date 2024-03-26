using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Vaccin
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Guid VaccinsManufacturerID { get; set; }
        [Required]
        public string PatientID { get; set; } = default!;
    }
}
