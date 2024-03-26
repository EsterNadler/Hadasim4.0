using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Illness
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime PositiveDate { get; set; }

        [Required]
        public DateTime NegativeDate { get; set; }

        [Required]
        public string PatientId { get; set; } = default!;
    }
}
