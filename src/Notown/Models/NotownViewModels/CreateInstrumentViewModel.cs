using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notown.Models.NotownViewModels
{
    public class CreateInstrumentViewModel
    {
        public Instrument Instrument { get; set; }

        public List<int> MusicianIDs { get; set; }

        //public IEnumerable<SelectListItem> MusicianIDs
        //{
        //    get
        //    {
        //        var items = new MultiSelectList(Instrument.Musicians, "ID", "Name");

        //        return items;
        //    }
        //}
        //public Musician Musician { get; set; }
    }
}
