using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Song
    {
        [Key]
        public int SongID { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The title cannot be longer than 30 characters.")]
        public string Title { get; set; }

        // Foreign Keys
        [Required]
        public int MusicianID { get; set; }
        [Required]
        public int? AlbumID { get; set; }

        public virtual Musician Musician { get; set; }
        public virtual Album Album { get; set; }
    }
}
