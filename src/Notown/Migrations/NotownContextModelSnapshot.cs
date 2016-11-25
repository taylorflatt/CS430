using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Notown.Data;

namespace Notown.Migrations
{
    [DbContext(typeof(NotownContext))]
    partial class NotownContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Notown.Models.Album", b =>
                {
                    b.Property<int>("ID");

                    b.Property<DateTime>("CopyrightDate");

                    b.Property<string>("MusicianSsn");

                    b.Property<string>("Name");

                    b.Property<int>("Speed");

                    b.HasKey("ID");

                    b.HasIndex("MusicianSsn");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("Notown.Models.Instrument", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Key");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Instrument");
                });

            modelBuilder.Entity("Notown.Models.Musician", b =>
                {
                    b.Property<string>("Ssn");

                    b.Property<int>("InstrumentID");

                    b.Property<string>("Name");

                    b.Property<string>("PlaceAddress");

                    b.Property<int>("uniqueID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Ssn");

                    b.HasIndex("InstrumentID");

                    b.HasIndex("PlaceAddress");

                    b.ToTable("Musician");
                });

            modelBuilder.Entity("Notown.Models.Place", b =>
                {
                    b.Property<string>("Address");

                    b.Property<string>("TelephoneNumber");

                    b.HasKey("Address");

                    b.HasIndex("TelephoneNumber")
                        .IsUnique();

                    b.ToTable("Place");
                });

            modelBuilder.Entity("Notown.Models.Song", b =>
                {
                    b.Property<int>("SongID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumID");

                    b.Property<string>("MusicianSsn");

                    b.Property<string>("Title");

                    b.HasKey("SongID");

                    b.HasIndex("AlbumID");

                    b.HasIndex("MusicianSsn");

                    b.ToTable("Song");
                });

            modelBuilder.Entity("Notown.Models.Telephone", b =>
                {
                    b.Property<string>("Number");

                    b.HasKey("Number");

                    b.ToTable("Telephone");
                });

            modelBuilder.Entity("Notown.Models.Album", b =>
                {
                    b.HasOne("Notown.Models.Musician", "Musician")
                        .WithMany("Albums")
                        .HasForeignKey("MusicianSsn");
                });

            modelBuilder.Entity("Notown.Models.Musician", b =>
                {
                    b.HasOne("Notown.Models.Instrument", "Instrument")
                        .WithMany("Musicians")
                        .HasForeignKey("InstrumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notown.Models.Place", "Place")
                        .WithMany("Musicians")
                        .HasForeignKey("PlaceAddress");
                });

            modelBuilder.Entity("Notown.Models.Place", b =>
                {
                    b.HasOne("Notown.Models.Telephone", "Telephone")
                        .WithOne("Place")
                        .HasForeignKey("Notown.Models.Place", "TelephoneNumber");
                });

            modelBuilder.Entity("Notown.Models.Song", b =>
                {
                    b.HasOne("Notown.Models.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notown.Models.Musician", "Musician")
                        .WithMany("Songs")
                        .HasForeignKey("MusicianSsn");
                });
        }
    }
}
