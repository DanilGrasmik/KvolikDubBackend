using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class ChangeAnimeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarImageUrl",
                table: "Animes",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Animes",
                newName: "AvatarImageUrl");
        }
    }
}
