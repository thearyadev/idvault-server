using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class PublicKeyStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PublicKey",
                value: "1232434556"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "PublicKey", table: "Users");
        }
    }
}
