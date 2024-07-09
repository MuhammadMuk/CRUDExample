﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(PersonsDbContext))]
    [Migration("20240708072524_InsertPerson_StordProcedure")]
    partial class InsertPerson_StordProcedure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"),
                            CountryName = "Syira"
                        },
                        new
                        {
                            CountryId = new Guid("98259d16-a976-42d9-96b2-0c35e071e0d1"),
                            CountryName = "Iraq"
                        },
                        new
                        {
                            CountryId = new Guid("aafa77fa-7e54-445c-bad0-3eb9efb4bd42"),
                            CountryName = "UAE"
                        },
                        new
                        {
                            CountryId = new Guid("ed358557-48dc-4e92-b033-a25ca76a408d"),
                            CountryName = "Kwit"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.HasKey("PersonID");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonID = new Guid("3e5755e7-b7bd-45dd-aa7c-46c555d254e9"),
                            Address = "PO Box 27811",
                            CountryID = new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"),
                            DateOfBirth = new DateTime(2015, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "dberrow0@topsy.com",
                            Gender = "Male",
                            PersonName = "Darren Berrow",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("410f43d4-c641-4b8d-90cd-46c703f3a5de"),
                            Address = "PO Box 27811",
                            CountryID = new Guid("98259d16-a976-42d9-96b2-0c35e071e0d1"),
                            DateOfBirth = new DateTime(1992, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "vofen1@bandcamp.com",
                            Gender = "Male",
                            PersonName = "Vick Ofen",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("0465ad34-7219-49f6-b0a9-b67918c6c6c8"),
                            Address = "6th Floor",
                            CountryID = new Guid("04de14b4-4bdf-48ae-9177-e369e87cc10c"),
                            DateOfBirth = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "cslight3@altervista.org",
                            Gender = "Female",
                            PersonName = "Colline Slight",
                            ReceiveNewsLetters = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}