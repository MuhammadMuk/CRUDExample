using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TIN_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIndentificationNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIndentificationNumber",
                table: "Persons",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC123456",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxIndentificationNumber",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldNullable: true,
                oldDefaultValue: "ABC123456");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("0465ad34-7219-49f6-b0a9-b67918c6c6c8"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("3e5755e7-b7bd-45dd-aa7c-46c555d254e9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("410f43d4-c641-4b8d-90cd-46c703f3a5de"),
                column: "TIN",
                value: null);
        }
    }
}
