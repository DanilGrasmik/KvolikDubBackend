using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class DeleteReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReviewEntityId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReviewEntityId",
                table: "Users",
                column: "ReviewEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Reviews_ReviewEntityId",
                table: "Users",
                column: "ReviewEntityId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Reviews_ReviewEntityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReviewEntityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReviewEntityId",
                table: "Users");
        }
    }
}
