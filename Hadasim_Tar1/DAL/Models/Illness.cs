using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
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
        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; } = default!;

        public Patient Patient { get; set; } = default!;
     

    }
}
