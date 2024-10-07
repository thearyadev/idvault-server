using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class DocumentImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Documents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Documents");
        }
    }
}
