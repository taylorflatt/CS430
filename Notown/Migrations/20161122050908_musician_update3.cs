using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class musician_update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_Musiciansid",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Musiciansid",
                table: "Songs");

            migrationBuilder.AddColumn<int>(
                name: "musicianIdForeignKey",
                table: "Songs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_musicianIdForeignKey",
                table: "Songs",
                column: "musicianIdForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Musicians_musicianIdForeignKey",
                table: "Songs",
                column: "musicianIdForeignKey",
                principalTable: "Musicians",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Musicians_musicianIdForeignKey",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_musicianIdForeignKey",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "musicianIdForeignKey",
                table: "Songs");

            migrationBuilder.AddColumn<int>(
                name: "Musiciansid",
                table: "Songs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_Musiciansid",
                table: "Songs",
                column: "Musiciansid");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs",
                column: "Musiciansid",
                principalTable: "Musicians",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
