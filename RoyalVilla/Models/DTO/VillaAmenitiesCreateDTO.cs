using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;


namespace RoyalVilla.Models.DTO
{
    public class VillaAmenitiesCreateDTO
    {
        

        [Required]
        [MaxLength(100)]
        public required string Name {get; set;}

        public string? Description {get; set;} 
        
      [Required]
        public int VillaId {get; set;}

    }
}