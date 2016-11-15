using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown1.Models
{
    public class Songs
    {
        [Key]
        public int songId { get; set; }

        [StringLength(30, ErrorMessage = "The author must be no longer than 30 characters.")]
        public string author { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        public string title { get; set; }

        [ForeignKey("AlbumProducerViewModel")]
        public int albumId { get; set; }

        public virtual AlbumProducerViewModel AlbumProducerViewModel { get; set; }
    }
}