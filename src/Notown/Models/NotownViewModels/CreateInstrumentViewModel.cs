﻿using System.Collections.Generic;

namespace Notown.Models.NotownViewModels
{
    public class CreateInstrumentViewModel
    {
        public Instrument Instrument { get; set; }
        public List<int> MusicianIDs { get; set; }
    }
}
