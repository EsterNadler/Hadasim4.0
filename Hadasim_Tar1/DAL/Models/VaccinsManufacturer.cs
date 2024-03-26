using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class VaccinsManufacturer
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
