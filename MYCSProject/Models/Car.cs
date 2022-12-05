using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MYCSProject.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Number { get; set; }
    }
}
