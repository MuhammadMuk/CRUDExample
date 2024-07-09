using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
	public class PersonsDbContext : DbContext
	{
		public PersonsDbContext(DbContextOptions options): base(options) { }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Person> Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");
			
			modelBuilder.Entity<Country>().HasData(new Country() { CountryId = Guid.Parse("04DE14B4-4BDF-48AE-9177-E369E87CC10C"), CountryName = "Syira" });
			modelBuilder.Entity<Country>().HasData(new Country() { CountryId = Guid.Parse("98259D16-A976-42D9-96B2-0C35E071E0D1"), CountryName = "Iraq" });
			modelBuilder.Entity<Country>().HasData(new Country() { CountryId = Guid.Parse("AAFA77FA-7E54-445C-BAD0-3EB9EFB4BD42"), CountryName = "UAE" });
			modelBuilder.Entity<Country>().HasData(new Country() { CountryId = Guid.Parse("ED358557-48DC-4E92-B033-A25CA76A408D"), CountryName = "Kwit" });
	
			modelBuilder.Entity<Person>().HasData(new Person()
			{
				PersonID = Guid.Parse("3E5755E7-B7BD-45DD-AA7C-46C555D254E9"),
				PersonName = "Darren Berrow",
				Address = "PO Box 27811",
				Email = "dberrow0@topsy.com",
				DateOfBirth = DateTime.Parse("8/1/2015"),
				Gender = "Male",
				ReceiveNewsLetters = false,
				CountryID = Guid.Parse("04DE14B4-4BDF-48AE-9177-E369E87CC10C")
			});
			modelBuilder.Entity<Person>().HasData(new Person()
			{
				PersonID = Guid.Parse("410F43D4-C641-4B8D-90CD-46C703F3A5DE"),
				PersonName = "Vick Ofen",
				Address = "PO Box 27811",
				Email = "vofen1@bandcamp.com",
				DateOfBirth = DateTime.Parse("2/2/1992"),
				Gender = "Male",
				ReceiveNewsLetters = true,
				CountryID = Guid.Parse("98259D16-A976-42D9-96B2-0C35E071E0D1")
			});
			modelBuilder.Entity<Person>().HasData(new Person()
			{
				PersonID = Guid.Parse("0465AD34-7219-49F6-B0A9-B67918C6C6C8"),
				PersonName = "Colline Slight",
				Address = "6th Floor",
				Email = "cslight3@altervista.org",
				DateOfBirth = DateTime.Parse("9/18/2023"),
				Gender = "Female",
				ReceiveNewsLetters = false,
				CountryID = Guid.Parse("04DE14B4-4BDF-48AE-9177-E369E87CC10C")
			});

			modelBuilder.Entity<Person>().Property(temp => temp.TIN).HasColumnName("TaxIndentificationNumber")
				.HasColumnType("varchar(8)")
				.HasDefaultValue("ABC123456");

			//modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();
			modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIndentificationNumber]) = 8");
		}
		public List<Person> sp_GetAllPersons()
		{
			return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
		}
		public int sp_InsertPerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[] { 
				new SqlParameter("@PersonID",person.PersonID),
				new SqlParameter("@PersonName",person.PersonName),
				new SqlParameter("@Email",person.Email),
				new SqlParameter("@DateOfBirth",person.DateOfBirth),
				new SqlParameter("@Gender",person.Gender),
				new SqlParameter("@CountryID",person.CountryID),
				new SqlParameter("@Address",person.Address),
				new SqlParameter("@ReceiveNewsLetters",person.ReceiveNewsLetters),
			};
			return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] " +
				"@PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryID," +
				"@Address,@ReceiveNewsLetters",parameters);
		}
	}
}
