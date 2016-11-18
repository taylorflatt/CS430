using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Musicians_Musiciansid1",
                table: "Album");

            migrationBuilder.DropIndex(
                name: "IX_Album_Musiciansid1",
                table: "Album");

            migrationBuilder.DropColumn(
                name: "Musiciansid1",
                table: "Album");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Musiciansid1",
                table: "Album",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Album_Musiciansid1",
                table: "Album",
                column: "Musiciansid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Musicians_Musiciansid1",
                table: "Album",
                column: "Musiciansid1",
                principalTable: "Musicians",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
