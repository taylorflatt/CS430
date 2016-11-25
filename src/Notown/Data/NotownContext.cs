using Microsoft.EntityFrameworkCore;
using Notown.Models;

namespace Notown.Data
{
    public class NotownContext : DbContext
    {
        public NotownContext(DbContextOptions<NotownContext> options)
            : base(options)
        {
        }

        public DbSet<Album> Album { get; set; }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<Musician> Musician { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<Telephone> Telephone { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Album>().ToTable("Album");
            builder.Entity<Instrument>().ToTable("Instrument");
            builder.Entity<Musician>().ToTable("Musician");
            builder.Entity<Place>().ToTable("Place");
            builder.Entity<Song>().ToTable("Song");
            builder.Entity<Telephone>().ToTable("Telephone");

            builder.Entity<Telephone>()
                .HasOne(p => p.Place)
                .WithOne(t => t.Telephone)
                .HasForeignKey<Place>(n => n.TelephoneNumber);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
