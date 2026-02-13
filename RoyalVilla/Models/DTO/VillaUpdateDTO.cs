using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace RoyalVilla.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id {get; set;}
        
        [MaxLength(50)]
        [Required]
        public required string Name {get; set;}

        public string? Details {get; set;} 

        public double Rate {get; set;}

        public int Sqft {get; set;}

        public int Occupancy {get; set;}

        public string? ImageUrl {get; set;}

    }
}