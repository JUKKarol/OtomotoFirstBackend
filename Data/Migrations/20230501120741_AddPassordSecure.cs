using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtomotoSimpleBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPassordSecure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Owners");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Owners",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Owners",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Owners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Owners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VeryficationToken",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "VeryficationToken",
                table: "Owners");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Owners",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
