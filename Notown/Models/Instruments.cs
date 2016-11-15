using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown1.Models
{
    public class Instruments
    {
        [Key]
        [StringLength(10, ErrorMessage = "The instrument ID must be no longer than 10 characters.")]
        public string instrumentId { get; set; }

        [StringLength(30, ErrorMessage = "The dname must be no longer than 30 characters.")]
        public string dName { get; set; }

        [StringLength(5, ErrorMessage = "The particular instrument key must be no longer than 5 characters.")]
        public string key { get; set; }
    }
}