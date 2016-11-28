using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown.Models
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(30, ErrorMessage = "The album name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        public int Speed { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Copyright Date")]
        public DateTime CopyrightDate { get; set; }

        // Foreign Key
        public int MusicianID { get; set; }

        public virtual Musician Musician { get; set; }      // Single Album Producer
        public virtual ICollection<Song> Songs { get; set; }
    }
}