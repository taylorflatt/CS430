using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Notown.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    albumIdentifier = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CopyrightDate = table.Column<DateTime>(nullable: false),
                    speed = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.albumIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    instrumentId = table.Column<string>(maxLength: 10, nullable: false),
                    dName = table.Column<string>(maxLength: 30, nullable: true),
                    key = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.instrumentId);
                });

            migrationBuilder.CreateTable(
                name: "Musicians",
                columns: table => new
                {
                    ssn = table.Column<string>(maxLength: 10, nullable: false),
                    name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicians", x => x.ssn);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    address = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.address);
                });

            migrationBuilder.CreateTable(
                name: "AlbumProducerViewModel",
                columns: table => new
                {
                    albumId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    copyrightDate = table.Column<DateTime>(nullable: false),
                    speed = table.Column<int>(nullable: false),
                    ssn = table.Column<string>(maxLength: 10, nullable: true),
                    title = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumProducerViewModel", x => x.albumId);
                    table.ForeignKey(
                        name: "FK_AlbumProducerViewModel_Musicians_ssn",
                        column: x => x.ssn,
                        principalTable: "Musicians",
                        principalColumn: "ssn",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Telephone",
                columns: table => new
                {
                    phone = table.Column<string>(maxLength: 1, nullable: false),
                    address = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telephone", x => x.phone);
                    table.ForeignKey(
                        name: "FK_Telephone_Place_address",
                        column: x => x.address,
                        principalTable: "Place",
                        principalColumn: "address",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    songId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    albumId = table.Column<int>(nullable: false),
                    albumIdentifier = table.Column<int>(nullable: true),
                    author = table.Column<string>(maxLength: 30, nullable: true),
                    title = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.songId);
                    table.ForeignKey(
                        name: "FK_Songs_AlbumProducerViewModel_albumId",
                        column: x => x.albumId,
                        principalTable: "AlbumProducerViewModel",
                        principalColumn: "albumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Songs_Album_albumIdentifier",
                        column: x => x.albumIdentifier,
                        principalTable: "Album",
                        principalColumn: "albumIdentifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumProducerViewModel_ssn",
                table: "AlbumProducerViewModel",
                column: "ssn");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_albumId",
                table: "Songs",
                column: "albumId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_albumIdentifier",
                table: "Songs",
                column: "albumIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Telephone_address",
                table: "Telephone",
                column: "address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Telephone");

            migrationBuilder.DropTable(
                name: "AlbumProducerViewModel");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Musicians");

            migrationBuilder.CreateTable(
                name: "NotownViewModel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotownViewModel", x => x.id);
                });
        }
    }
}
