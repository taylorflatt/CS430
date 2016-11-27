using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models.NotownViewModels
{
    public class CreateMusicianViewModel
    {
        public Musician Musician { get; set; }
        public Instrument Instrument { get; set; }
    }
}
