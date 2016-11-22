using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class musician_update : Migration
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
                name: "songForeignKey",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "songForeignKey",
                table: "Musicians",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_songForeignKey",
                table: "Songs",
                column: "songForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Musicians_songForeignKey",
                table: "Songs",
                column: "songForeignKey",
                principalTable: "Musicians",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Musicians_songForeignKey",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_songForeignKey",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "songForeignKey",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "songForeignKey",
                table: "Musicians");

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
