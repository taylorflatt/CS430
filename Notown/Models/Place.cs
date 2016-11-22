using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown.Models
{
    public class Place
    {
        [Key]
        [StringLength(30, ErrorMessage = "The address must be no longer than 30 characters.")]
        public string address { get; set; }

        [StringLength(1, ErrorMessage = "The telephone must be no longer than 11 characters.")]
        public string phone { get; set; }

        [ForeignKey("phone")]
        public virtual Telephone Telephone { get; set; }
        public virtual IEnumerable<Musicians> Musicians { get; set; }
    }
}