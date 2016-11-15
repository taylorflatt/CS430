using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown1.Models
{
    public class Album
    {
        [Key]
        public int albumIdentifier { get; set; }
        public int speed { get; set; }
        public DateTime CopyrightDate { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        public string title { get; set; }

        public virtual ICollection<Songs> Songs { get; set; }
    }
}