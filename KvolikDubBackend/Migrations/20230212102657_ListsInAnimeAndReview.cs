using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class ListsInAnimeAndReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "LikedUsersEmails",
                table: "Reviews",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Frames",
                table: "Animes",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Genres",
                table: "Animes",
                type: "text[]",
                nullable: true);
        }

        /*protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedUsersEmails",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Frames",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Animes");
        }*/
    }
}
