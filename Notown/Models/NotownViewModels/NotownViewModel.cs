using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notown1.Models
{
    public class NotownViewModel
    {
        [Key]
        public int id { get; set; }
    }

    public class PlaysViewModel
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Musicians")]
        [StringLength(10, ErrorMessage = "The ssn must be no longer than 10 characters.")]
        string ssn { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Instruments")]
        [StringLength(30, ErrorMessage = "The instrument ID must be no longer than 30 characters.")]
        string instrumentId { get; set; }

        public virtual Musicians Musicians { get; set; }
        public virtual Instruments Instruments { get; set; }
    }

    public class PerformViewModel
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Songs")]
        public int songId { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Musicians")]
        [StringLength(10, ErrorMessage = "The ssn must be no longer than 10 characters.")]
        public string ssn { get; set; }

        public virtual Songs Songs { get; set; }
        public virtual Musicians Musicians { get; set; }
    }

    public class AlbumProducerViewModel
    {
        [Key]
        public int albumId { get; set; }

        [ForeignKey("Musicians")]
        [StringLength(10, ErrorMessage = "The ssn must be no longer than 10 characters.")]
        public string ssn { get; set; }

        public DateTime copyrightDate { get; set; }
        public int speed { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        public string title { get; set; }

        public virtual Musicians Musicians { get; set; }
    }

    public class LivesViewModel
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Musicians")]
        [StringLength(10, ErrorMessage = "The ssn must be no longer than 10 characters.")]
        public string ssn { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Place")]
        [StringLength(30, ErrorMessage = "The address must be no longer than 30 characters.")]
        public string address { get; set; }

        [ForeignKey("Telephone")]
        [StringLength(11, ErrorMessage = "The phone number must be no longer than 11 characters.")]
        public string phone { get; set; }

        public virtual Musicians Musicians { get; set; }
        public virtual Place Place { get; set; }
        public virtual Telephone Telephone { get; set; }
    }
}