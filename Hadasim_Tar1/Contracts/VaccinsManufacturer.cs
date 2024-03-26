using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class VaccinsManufacturer//public for other projects
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
