using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Instrument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Instrument ID")]
        public int ID { get; set; }

        [MaxLength(30, ErrorMessage = "The instrument name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        [Display(Name = "Musical Key")]
        [MaxLength(5, ErrorMessage = "The Key cannot be longer than 5 characters.")]
        public string Key { get; set; }

        public virtual ICollection<Musician> Musicians { get; set; }
    }
}
