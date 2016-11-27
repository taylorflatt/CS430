using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models.NotownViewModels
{
    public class EditMusicianViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Ssn { get; set; }

        public string Name { get; set; }

        // Foreign Keys
        public int PlaceID { get; set; }
        public int InstrumentID { get; set; }

        public virtual Place Place { get; set; }
        public virtual Instrument Instrument { get; set; }
    }
}
