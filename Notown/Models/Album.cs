using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Notown.Data;
using FluentValidation.Attributes;

namespace Notown.Models
{
    [Validator(typeof(AlbumValidator))]
    public class Album
    {
        [Key]
        public int albumID { get; set; }

        public int speed { get; set; }

        [DataType(DataType.Date)]
        public DateTime CopyrightDate { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        public string title { get; set; }
        public string producer { get; set; }
        
        public virtual ICollection<Songs> Songs { get; set; }
        public virtual Musicians Musicians { get; set; }
    }

    public class AlbumValidator : AbstractValidator<Album>
    {
        private readonly ApplicationDbContext _context;

        public AlbumValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(album => album.producer).Must(ProducerExists).WithMessage("The producer must exist in the Artists catalog first.");
        }

        // Check if a musician in the Musicians table has the name of the producer we are trying to add to an album.
        private bool ProducerExists(string value)
        {
            return _context.Musicians.Any(p => p.name == value);
        }
    }

}