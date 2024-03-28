using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class Patient
    {
        [StringLength(9)]
        public string Id { get; set; } = default!;

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; } = default!;
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        public string Address { get; set; } = default!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Phone]
        [MaxLength(9)]
        public string? Phone { get; set; }
        [Required]
        [Phone]
        [MaxLength(10)]
        public string CellPhone { get; set; } = default!;

        public IFormFile Image { get; set; } = default!;
    }
}
