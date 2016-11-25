using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }

        [DataType(DataType.Date)]
        public DateTime CopyrightDate { get; set; }

        // Foreign Key
        public string MusicianSsn { get; set; }

        public virtual Musician Musician { get; set; }      // Single Album Producer
        public virtual ICollection<Song> Songs { get; set; }
    }
}