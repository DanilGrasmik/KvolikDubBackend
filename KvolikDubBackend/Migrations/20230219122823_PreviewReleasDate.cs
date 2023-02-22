using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class PreviewReleasDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseFrom",
                table: "Previews",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseFrom",
                table: "Previews");
        }
    }
}
