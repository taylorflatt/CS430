//using System.Linq;
//using FluentValidation;
//using Notown.Data;
//using Notown.Models;

//namespace Notown.Validators
//{
//    public class AlbumValidator : AbstractValidator<Album>
//    {
//        private readonly ApplicationDbContext _context;

//        public AlbumValidator(ApplicationDbContext context)
//        {
//            _context = context;

//            RuleFor(album => album.producer).Must(ProducerExists).WithMessage("The producer must exist in the Artists catalog first.");
//        }

//        // Check if a musician in the Musicians table has the name of the producer we are trying to add to an album.
//        private bool ProducerExists(string value)
//        {
//            return _context.Musicians.Any(p => p.name == value);
//        }
//    }
//}
