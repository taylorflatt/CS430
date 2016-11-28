using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Musician
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Programmatically ensured to be unique at time of creation (and any subsequent edits).
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name="SSN")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "The SSN must be exactly 9 digits.")]
        public string Ssn { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        // Foreign Keys
        [Required]
        public int PlaceID { get; set; }
        [Required]
        public int InstrumentID { get; set; }

        public virtual Place Place { get; set; }
        public virtual Instrument Instrument { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
