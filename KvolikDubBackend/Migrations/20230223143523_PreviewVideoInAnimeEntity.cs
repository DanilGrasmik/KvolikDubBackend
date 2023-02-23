using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class PreviewVideoInAnimeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewVideoUrl",
                table: "Animes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewVideoUrl",
                table: "Animes");
        }
    }
}
