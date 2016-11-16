using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class producer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "producer",
                table: "Album",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Musiciansid",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs",
                column: "Musiciansid",
                principalTable: "Musicians",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "producer",
                table: "Album");

            migrationBuilder.AlterColumn<int>(
                name: "Musiciansid",
                table: "Songs",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Musicians_Musiciansid",
                table: "Songs",
                column: "Musiciansid",
                principalTable: "Musicians",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
