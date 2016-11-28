using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Song
    {
        [Key]
        public int SongID { get; set; }

        [MaxLength(30, ErrorMessage = "The title cannot be longer than 30 characters.")]
        public string Title { get; set; }

        // Foreign Keys
        public int MusicianID { get; set; }
        public int? AlbumID { get; set; }

        public virtual Musician Musician { get; set; }
        public virtual Album Album { get; set; }
    }
}
