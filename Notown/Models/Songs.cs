using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown.Models
{
    public class Songs
    {
        [Key]
        public int songId { get; set; }

        [StringLength(30, ErrorMessage = "The author must be no longer than 30 characters.")]
        [Display(Name = "Author", Description = "The name of artist who created this song.")]
        public string name { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        [Display(Name = "Song Title")]
        public string title { get; set; }

        [Display(Name = "Album ID", Description = "The specific ID given to each unique album which contains this song.")]
        public int albumIdForeignKey { get; set; }

        [ForeignKey("albumIdForeignKey")]
        public virtual Album Album { get; set; }

        public virtual Musicians Musicians { get; set; }
    }
}