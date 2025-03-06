using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //Won't automatically generate this as an identity column
        [Display(Name = "Villa Room Number")]
        public int Villa_Number { get; set; }
        [ForeignKey("Villa")]
        [Display(Name = "Villa")]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }
        [Display(Name = "Special Details")]
        public string? SpecialDetails { get; set; }
    }
}
