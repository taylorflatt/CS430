using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notown.Migrations
{
    public partial class musician_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ssn",
                table: "Musician",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Musician",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ssn",
                table: "Musician",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Musician",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }
    }
}
