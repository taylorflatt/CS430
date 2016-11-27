using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(30, ErrorMessage = "The address cannot be longer than 30 characters.")]
        public string Address { get; set; }

        // Foreign Key
        public string TelephoneNumber { get; set; }

        public virtual Telephone Telephone { get; set; }
        public virtual ICollection<Musician> Musicians { get; set; }
    }
}
