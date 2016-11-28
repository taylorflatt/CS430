using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown.Models.NotownViewModels
{
    public class EditPlaceViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Address { get; set; }

        // Foreign Key
        public string TelephoneNumber { get; set; }

        public virtual Telephone Telephone { get; set; }
    }
}
