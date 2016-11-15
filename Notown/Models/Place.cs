using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown1.Models
{
    public class Place
    {
        [Key]
        [StringLength(30, ErrorMessage = "The address must be no longer than 30 characters.")]
        public string address { get; set; }
    }
}