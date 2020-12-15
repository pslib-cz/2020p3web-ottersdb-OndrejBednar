using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OttersDatabase.Models
{
    public class Otter
    {
        public string Name { get; set; }
        public string Color { get; set; }
        [Key]
        public int? TattooID { get; set; }
        public Otter Mother { get; set; }
        [Display(Name = "Matka")]
        [ForeignKey("Mother")]
        public int? MotherId { get; set; }
        [Required]
        public Place Place { get; set; }
        public string PlaceName { get; set; }
        public Location Location { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public ICollection<Otter> Children { get; set; }
        public IdentityUser founder { get; set; }
        [ForeignKey("founder")]
        public string founderID { get; set; }
    }
}
