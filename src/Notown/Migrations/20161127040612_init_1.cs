using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class init_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_Musician_MusicianID",
                table: "Song");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song",
                column: "AlbumID",
                principalTable: "Album",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Musician_MusicianID",
                table: "Song",
                column: "MusicianID",
                principalTable: "Musician",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_Musician_MusicianID",
                table: "Song");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song",
                column: "AlbumID",
                principalTable: "Album",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Musician_MusicianID",
                table: "Song",
                column: "MusicianID",
                principalTable: "Musician",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
