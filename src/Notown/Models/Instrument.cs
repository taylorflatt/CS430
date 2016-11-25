using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models
{
    public class Instrument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Musical Key")]
        public string Key { get; set; }

        public virtual ICollection<Musician> Musicians { get; set; }
    }
}
