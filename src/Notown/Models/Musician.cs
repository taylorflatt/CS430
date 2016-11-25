using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Musician
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Ssn { get; set; }

        // Used for the unique url link so the SSN isn't displayed.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int uniqueID { get; set; }

        public string Name { get; set; }

        // Foreign Keys
        public string PlaceAddress { get; set; }
        public int InstrumentID { get; set; }

        public virtual Place Place { get; set; }
        public virtual Instrument Instrument { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
