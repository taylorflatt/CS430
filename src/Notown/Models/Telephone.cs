using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Telephone
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Telephone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The telephone number can only be 10 digits long.")]
        public string Number { get; set; }

        public virtual Place Place { get; set; }
    }
}
