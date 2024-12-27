using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlexIn.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Businesss",
                table: "Businesss");

            migrationBuilder.RenameTable(
                name: "Businesss",
                newName: "Businesses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses");

            migrationBuilder.RenameTable(
                name: "Businesses",
                newName: "Businesss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Businesss",
                table: "Businesss",
                column: "Id");
        }
    }
}
