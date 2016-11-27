using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Notown.Data;

namespace Notown.Migrations
{
    [DbContext(typeof(NotownContext))]
    [Migration("20161127040840_album_1")]
    partial class album_1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Notown.Models.Album", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CopyrightDate");

                    b.Property<int>("MusicianID");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<int>("Speed");

                    b.HasKey("ID");

                    b.HasIndex("MusicianID");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("Notown.Models.Instrument", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Key")
                        .HasMaxLength(5);

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.HasKey("ID");

                    b.ToTable("Instrument");
                });

            modelBuilder.Entity("Notown.Models.Musician", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("InstrumentID");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<int>("PlaceID");

                    b.Property<string>("Ssn")
                        .HasMaxLength(9);

                    b.HasKey("ID");

                    b.HasIndex("InstrumentID");

                    b.HasIndex("PlaceID");

                    b.HasIndex("Ssn")
                        .IsUnique();

                    b.ToTable("Musician");
                });

            modelBuilder.Entity("Notown.Models.Place", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(30);

                    b.Property<string>("TelephoneNumber");

                    b.HasKey("ID");

                    b.HasIndex("TelephoneNumber")
                        .IsUnique();

                    b.ToTable("Place");
                });

            modelBuilder.Entity("Notown.Models.Song", b =>
                {
                    b.Property<int>("SongID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AlbumID");

                    b.Property<int>("MusicianID");

                    b.Property<string>("Title")
                        .HasMaxLength(30);

                    b.HasKey("SongID");

                    b.HasIndex("AlbumID");

                    b.HasIndex("MusicianID");

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
                        .HasForeignKey("MusicianID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notown.Models.Musician", b =>
                {
                    b.HasOne("Notown.Models.Instrument", "Instrument")
                        .WithMany("Musicians")
                        .HasForeignKey("InstrumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notown.Models.Place", "Place")
                        .WithMany("Musicians")
                        .HasForeignKey("PlaceID")
                        .OnDelete(DeleteBehavior.Cascade);
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
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Notown.Models.Musician", "Musician")
                        .WithMany("Songs")
                        .HasForeignKey("MusicianID");
                });
        }
    }
}
