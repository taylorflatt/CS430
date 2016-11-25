using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models
{
    public class Song
    {
        [Key]
        public int SongID { get; set; }
        public string Title { get; set; }

        // Foreign Keys
        public string MusicianSsn { get; set; }
        public int AlbumID { get; set; }

        public virtual Musician Musician { get; set; }
        public virtual Album Album { get; set; }
    }
}
