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
        public string ssn { get; set; }

        [StringLength(30, ErrorMessage = "The name must be no longer than 30 characters.")]
        public string name { get; set; }

        public virtual ICollection<Album> Album { get; set; }
    }
}