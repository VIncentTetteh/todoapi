using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class MadeEmailAndPhoneNumberUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_PhoneNumber",
                table: "Users",
                columns: new[] { "Email", "PhoneNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email_PhoneNumber",
                table: "Users");
        }
    }
}
