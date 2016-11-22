using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Instruments
    {
        [Key]
        public int instruId { get; set; }

        [StringLength(30, ErrorMessage = "The dname must be no longer than 30 characters.")]
        public string dName { get; set; }

        [StringLength(5, ErrorMessage = "The particular instrument key must be no longer than 5 characters.")]
        public string key { get; set; }

        public virtual Musicians Musician { get; set; }
    }
}