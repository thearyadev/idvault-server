using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Name", "Password", "PublicKey", "Username" },
                values: new object[] { "User 1", "user1", "", "user1" }
            );

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                    "UserId",
                    "Email",
                    "Name",
                    "Password",
                    "PhoneNumber",
                    "PublicKey",
                    "Username",
                },
                values: new object[]
                {
                    2,
                    "user2@user2.com",
                    "User 2",
                    "user2",
                    "6473310099",
                    "",
                    "user2",
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Users", keyColumn: "UserId", keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Name", "Password", "PublicKey", "Username" },
                values: new object[] { "testuser", "testuser", "1232434556", "testuser" }
            );
        }
    }
}
