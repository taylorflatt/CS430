using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notown.Models;
using Notown.Models.NotownViewModels;

namespace Notown.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Album> Album { get; set; }
        public DbSet<Instruments> Instruments { get; set; }
        public DbSet<Musicians> Musicians { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<Songs> Songs { get; set; }
        public DbSet<Telephone> Telephone { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Album>().ToTable("Album");
            builder.Entity<Instruments>().ToTable("Instruments");
            builder.Entity<Musicians>().ToTable("Musicians");
            builder.Entity<Place>().ToTable("Place");
            builder.Entity<Songs>().ToTable("Songs");
            builder.Entity<Telephone>().ToTable("Telephone");

            // Each song can have only 1 album.
            builder.Entity<Songs>()
                .HasOne(a => a.Album)
                .WithMany(s => s.Songs)
                .HasForeignKey(a => a.albumIdForeignKey);

            // Each album can have many songs.
            builder.Entity<Album>()
                .HasMany(s => s.Songs)
                .WithOne(a => a.Album)
                .HasPrincipalKey(s => s.albumID);

            //builder.Entity<Musicians>()
            //    .HasMany(s => s.Song)
            //    .WithOne(m => m.Musicians)
            //    .HasPrincipalKey(s => s.id);

            // For each home, there can be many musicians.
            builder.Entity<Musicians>()
                .HasOne(h => h.Place)
                .WithMany(m => m.Musicians)
                .HasForeignKey(h => h.placeForeignKey);

            // Every place has only 1 telephone number.
            builder.Entity<Place>()
                .HasOne(t => t.Telephone)
                .WithOne(p => p.Place);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
