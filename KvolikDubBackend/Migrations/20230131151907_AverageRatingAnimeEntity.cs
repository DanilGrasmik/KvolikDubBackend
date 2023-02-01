using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class AverageRatingAnimeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Animes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Animes");
        }
    }
}
