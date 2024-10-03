using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Password", "PhoneNumber", "Username" },
                values: new object[]
                {
                    "test@testuser.com",
                    "testuser",
                    "testuser",
                    "4169973041",
                    "testuser",
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Password", "PhoneNumber", "Username" },
                values: new object[]
                {
                    "aryan@aryankothari.dev",
                    "Aryan Kothari",
                    "password1",
                    "64777610177",
                    "arya",
                }
            );
        }
    }
}
