using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace RoyalVilla.Models.DTO
{
    public class VillaAmenitiesUpdateDTO
    {
        [Key]
        public int Id {get ; set;}

        [Required]
        [MaxLength(100)]
        public required string Name {get; set;}

        public string? Description {get; set;} 
        
        [Required]
        public int VillaId {get; set;}


    }
}