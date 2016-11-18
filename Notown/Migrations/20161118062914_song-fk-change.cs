using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class songfkchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Album_albumId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_albumId",
                table: "Songs");

            migrationBuilder.AddColumn<int>(
                name: "albumIdForeignKey",
                table: "Songs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "albumId",
                table: "Songs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_albumIdForeignKey",
                table: "Songs",
                column: "albumIdForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Album_albumIdForeignKey",
                table: "Songs",
                column: "albumIdForeignKey",
                principalTable: "Album",
                principalColumn: "albumID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Album_albumIdForeignKey",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_albumIdForeignKey",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "albumIdForeignKey",
                table: "Songs");

            migrationBuilder.AlterColumn<int>(
                name: "albumId",
                table: "Songs",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_albumId",
                table: "Songs",
                column: "albumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Album_albumId",
                table: "Songs",
                column: "albumId",
                principalTable: "Album",
                principalColumn: "albumID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
