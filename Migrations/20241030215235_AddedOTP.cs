using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpExpiryTime",
                table: "Users");
        }
    }
}
