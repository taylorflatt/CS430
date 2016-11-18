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

            builder.Entity<Songs>()
                .HasOne(a => a.Album)
                .WithMany(s => s.Songs)
                .HasForeignKey(a => a.albumIdForeignKey);

            builder.Entity<Album>()
                .HasMany(s => s.Songs)
                .WithOne(a => a.Album)
                .HasPrincipalKey(s => s.albumID);



            //builder.Entity<Album>()
            //    .HasMany(s => s.Songs)
            //    .WithOne(a => a.Album)
            //    .HasForeignKey(s => s.albumId);

            //builder.Entity<Songs>()
            //    .HasOne(m => m.Musicians)
            //    .WithMany();

            //builder.Entity<Musicians>()
            //    .HasMany(a => a.Album)
            //    .WithOne(m => m.Musicians);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
