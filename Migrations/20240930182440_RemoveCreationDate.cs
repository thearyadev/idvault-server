using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvault_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CreationDate", table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreationDate",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: ""
            );
        }
    }
}
