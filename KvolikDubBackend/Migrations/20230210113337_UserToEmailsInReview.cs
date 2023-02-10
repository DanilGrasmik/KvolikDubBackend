using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    public partial class UserToEmailsInReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<List<string>>(
                name: "LikedUsersEmails",
                table: "Reviews",
                type: "text[]",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedUsersEmails",
                table: "Reviews");

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
    }
}
