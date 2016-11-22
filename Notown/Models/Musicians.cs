using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown.Models
{
    public class Musicians
    {
        [Key]
        public int id { get; set; }

        [StringLength(10, ErrorMessage ="The SSN must be no longer than 10 characters.")]
        [Required]
        public string ssn { get; set; }

        [StringLength(30, ErrorMessage = "The name must be no longer than 30 characters.")]
        [Required]
        public string name { get; set; }

        [StringLength(30, ErrorMessage = "The address must be no longer than 30 characters.")]
        public string placeForeignKey { get; set; }

        public virtual Instruments Instrument { get; set; }

        [ForeignKey("placeForeignKey")]
        public virtual Place Place { get; set; }

        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Songs> Song { get; set; }
    }
}