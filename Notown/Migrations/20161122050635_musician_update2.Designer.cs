using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Notown.Data;

namespace Notown.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161122050635_musician_update2")]
    partial class musician_update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Notown.Models.Album", b =>
                {
                    b.Property<int>("albumID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CopyrightDate");

                    b.Property<int>("producer");

                    b.Property<int>("speed");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("albumID");

                    b.HasIndex("producer");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("Notown.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Notown.Models.Instruments", b =>
                {
                    b.Property<int>("instruId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("dName")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("key")
                        .HasAnnotation("MaxLength", 5);

                    b.HasKey("instruId");

                    b.ToTable("Instruments");
                });

            modelBuilder.Entity("Notown.Models.Musicians", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("InstrumentinstruId");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("placeForeignKey")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("ssn")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.HasKey("id");

                    b.HasIndex("InstrumentinstruId")
                        .IsUnique();

                    b.HasIndex("placeForeignKey");

                    b.ToTable("Musicians");
                });

            modelBuilder.Entity("Notown.Models.Place", b =>
                {
                    b.Property<string>("address")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("phone")
                        .HasAnnotation("MaxLength", 1);

                    b.HasKey("address");

                    b.HasIndex("phone")
                        .IsUnique();

                    b.ToTable("Place");
                });

            modelBuilder.Entity("Notown.Models.Songs", b =>
                {
                    b.Property<int>("songId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Musiciansid");

                    b.Property<int>("albumIdForeignKey");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("songId");

                    b.HasIndex("Musiciansid");

                    b.HasIndex("albumIdForeignKey");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Notown.Models.Telephone", b =>
                {
                    b.Property<string>("phone")
                        .HasAnnotation("MaxLength", 1);

                    b.HasKey("phone");

                    b.ToTable("Telephone");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Notown.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Notown.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notown.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notown.Models.Album", b =>
                {
                    b.HasOne("Notown.Models.Musicians", "Musicians")
                        .WithMany("Album")
                        .HasForeignKey("producer")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notown.Models.Musicians", b =>
                {
                    b.HasOne("Notown.Models.Instruments", "Instrument")
                        .WithOne("Musician")
                        .HasForeignKey("Notown.Models.Musicians", "InstrumentinstruId");

                    b.HasOne("Notown.Models.Place", "Place")
                        .WithMany("Musicians")
                        .HasForeignKey("placeForeignKey");
                });

            modelBuilder.Entity("Notown.Models.Place", b =>
                {
                    b.HasOne("Notown.Models.Telephone", "Telephone")
                        .WithOne("Place")
                        .HasForeignKey("Notown.Models.Place", "phone");
                });

            modelBuilder.Entity("Notown.Models.Songs", b =>
                {
                    b.HasOne("Notown.Models.Musicians", "Musicians")
                        .WithMany("Song")
                        .HasForeignKey("Musiciansid");

                    b.HasOne("Notown.Models.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("albumIdForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
