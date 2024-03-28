using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class PatientResponse
    {
        public string Id { get; set; } = default!;

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public string? Phone { get; set; }
        public string CellPhone { get; set; } = default!;
        public byte[]? Image { get; set; } = default!;
    }
}
