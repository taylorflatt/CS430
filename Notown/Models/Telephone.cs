﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown1.Models
{
    public class Telephone
    {
        [Key]
        [StringLength(1, ErrorMessage = "The telephone must be no longer than 11 characters.")]
        public string phone { get; set; }

        [ForeignKey("Place")]
        [StringLength(30, ErrorMessage = "The address must be no longer than 30 characters.")]
        public string address { get; set; }

        public virtual Place Place { get; set; }
    }
}