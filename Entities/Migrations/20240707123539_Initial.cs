using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"), "Syira" },
                    { new Guid("98259d16-a976-42d9-96b2-0c35e071e0d1"), "Iraq" },
                    { new Guid("aafa77fa-7e54-445c-bad0-3eb9efb4bd42"), "UAE" },
                    { new Guid("ed358557-48dc-4e92-b033-a25ca76a408d"), "Kwit" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("0465ad34-7219-49f6-b0a9-b67918c6c6c8"), "6th Floor", new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "cslight3@altervista.org", "Female", "Colline Slight", false },
                    { new Guid("3e5755e7-b7bd-45dd-aa7c-46c555d254e9"), "PO Box 27811", new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"), new DateTime(2015, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dberrow0@topsy.com", "Male", "Darren Berrow", false },
                    { new Guid("410f43d4-c641-4b8d-90cd-46c703f3a5de"), "PO Box 27811", new Guid("98259d16-a976-42d9-96b2-0c35e071e0d1"), new DateTime(1992, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "vofen1@bandcamp.com", "Male", "Vick Ofen", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
