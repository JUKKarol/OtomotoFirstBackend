using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtomotoSimpleBackend.Migrations
{
    /// <inheritdoc />
    public partial class CorrectNamesInOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastame",
                table: "Owners",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstame",
                table: "Owners",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Owners",
                newName: "Lastame");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Owners",
                newName: "Firstame");
        }
    }
}
