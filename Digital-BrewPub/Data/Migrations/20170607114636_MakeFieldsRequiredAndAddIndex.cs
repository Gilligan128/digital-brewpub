using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digital.BrewPub.Data.Migrations
{
    public partial class MakeFieldsRequiredAndAddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Brewery",
                table: "Notes",
                nullable: false,
                maxLength: 50,
                oldClrType: typeof(string));


            migrationBuilder.CreateIndex(
                name: "IX_Notes_Brewery",
                table: "Notes",
                column: "Brewery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_Brewery",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Brewery",
                table: "Notes",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
