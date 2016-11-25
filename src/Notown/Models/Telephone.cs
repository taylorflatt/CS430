using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notown.Models
{
    public class Telephone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Number { get; set; }

        public virtual Place Place { get; set; }
    }
}
