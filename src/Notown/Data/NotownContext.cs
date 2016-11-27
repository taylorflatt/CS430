using Microsoft.EntityFrameworkCore;
using Notown.Models;
using System.Data;

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

            // When an album is deleted, set the song reference to albums to null (so it doesn't refer to any album).
            builder.Entity<Song>()
                .HasOne(a => a.Album)
                .WithMany(s => s.Songs)
                .HasForeignKey(k => k.AlbumID)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.SetNull);

            // When a musician is deleted, deleted the songs associated with him.
            builder.Entity<Song>()
                .HasOne(m => m.Musician)
                .WithMany(s => s.Songs)
                .HasForeignKey(k => k.MusicianID)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            // When a musician is deleted, delete the albums associated with him.
            builder.Entity<Album>()
                .HasOne(p => p.Musician)
                .WithMany(a => a.Albums)
                .HasForeignKey(k => k.MusicianID)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            // Add an index so that the SSN is a unique value.
            builder.Entity<Musician>()
                .HasIndex(s => s.Ssn)
                .IsUnique();

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
