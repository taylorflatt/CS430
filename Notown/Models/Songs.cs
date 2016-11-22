using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notown.Data;
using FluentValidation;

namespace Notown.Models
{
    public class Songs
    {
        [Key]
        public int songId { get; set; }

        [StringLength(30, ErrorMessage = "The title must be no longer than 30 characters.")]
        [Display(Name = "Song Title")]
        [Required]
        public string title { get; set; }

        [Display(Name = "Album", Description = "The specific ID given to each unique album which contains this song.")]
        public int albumIdForeignKey { get; set; }

        //[StringLength(30, ErrorMessage = "The author must be no longer than 30 characters.")]
        [Display(Name = "Song Author", Description = "The name of artist who created this song.")]
        [Required]
        public int musicianIdForeignKey { get; set; }

        [ForeignKey("albumIdForeignKey")]
        public virtual Album Album { get; set; }

        [ForeignKey("musicianIdForeignKey")]
        public virtual Musicians Musicians { get; set; }
    }














    public class SongValidator : AbstractValidator<Songs>
    {
        private readonly ApplicationDbContext _context;

        public SongValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(song => song.albumIdForeignKey).Must(AlbumExists).WithMessage("The album must already exist in the database before we can add a song to it.");
        }

        // Check if a musician in the Musicians table has the name of the producer we are trying to add to an album.
        private bool AlbumExists(int value)
        {
            return _context.Album.Any(p => p.albumID == value);
        }
    }
}