using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveValidationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationStatus",
                table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidationStatus",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
