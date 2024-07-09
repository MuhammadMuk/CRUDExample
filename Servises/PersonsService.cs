
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Servises.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Servises
{
	public class PersonsService : IPersonsService
	{
		private readonly PersonsDbContext _db;
		private readonly ICountriesService _countrieService;

		public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
		{
			_db = personsDbContext;
			_countrieService = countriesService;
		}

		public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
		{
			if (personAddRequest == null)
			{
				throw new ArgumentNullException(nameof(personAddRequest));
			}
			ValidationHelper.ModelValidation(personAddRequest);

			Person person = personAddRequest.ToPerson();
			person.PersonID = Guid.NewGuid();
			_db.Persons.Add(person);
			await _db.SaveChangesAsync();
			//_db.sp_InsertPerson(person);

			return person.ToPersonResponse();

		}

		public async Task<List<PersonResponse>> GetAllPersons()
		{
			//return _db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
			var persons = await _db.Persons.Include("Country").ToListAsync();
			return persons.Select(temp => temp.ToPersonResponse()).ToList();
		}

		public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
		{
			if (personID == null)
			{
				return null;
			}
			Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personID);
			if (person == null)
			{
				return null;
			}
			return person.ToPersonResponse();
		}

		public async Task<List<PersonResponse>> GetFilterdPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = await GetAllPersons();
			List<PersonResponse> matchingPersons = allPersons;

			if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
			{
				return matchingPersons;
			}
			switch (searchBy)
			{
				case nameof(PersonResponse.PersonName):
					matchingPersons = allPersons.
							Where(temp =>
							(!string.IsNullOrEmpty(temp.PersonName)) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.Email):
					matchingPersons = allPersons.
							Where(temp =>
							(!string.IsNullOrEmpty(temp.Email)) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.DateOfBirth):
					matchingPersons = allPersons.
							Where(temp =>
							(temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd MM YYYY").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.Gender):
					matchingPersons = allPersons.
							Where(temp =>
							(!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.CountryID):
					matchingPersons = allPersons.
							Where(temp =>
							(!string.IsNullOrEmpty(temp.Country)) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.Address):
					matchingPersons = allPersons.
							Where(temp =>
							(!string.IsNullOrEmpty(temp.Address)) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				default: matchingPersons = allPersons; break;
			}
			return matchingPersons;
		}

		public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return allPersons;
			}
			List<PersonResponse> sortedPerson = (sortBy, sortOrder)
				switch
			{
				(nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
				(nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
				(nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
				_ => allPersons
			};
			return sortedPerson;
		}

		public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if (personUpdateRequest == null)
			{
				throw new ArgumentNullException(nameof(Person));
			}

			ValidationHelper.ModelValidation(personUpdateRequest);

			Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);

			if (matchingPerson == null)
			{
				throw new ArgumentException("Given Person ID not Exisit");
			}
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryID = personUpdateRequest.CountryID;
			matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

			await _db.SaveChangesAsync();

			return matchingPerson.ToPersonResponse();
		}

		public async Task<bool> DeletePerson(Guid? personID)
		{
			if (personID == null)
			{
				throw new ArgumentNullException(nameof(personID));
			}
			Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);
			if (person == null)
			{
				return false;
			}
			_db.Persons.Remove(_db.Persons.First(temp => temp.PersonID == personID));
			await _db.SaveChangesAsync();
			return true;
		}

		public async Task<MemoryStream> GetPersonsCSV()
		{
			var memoryStream = new MemoryStream();
			using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
			{
				using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
				{
					csvWriter.WriteHeader<PersonResponse>();
					csvWriter.NextRecord();
					var persons = _db.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();
					await csvWriter.WriteRecordsAsync(persons);
				}
				// Make sure all data is written to the MemoryStream
				await streamWriter.FlushAsync();
			}

			memoryStream.Position = 0;
			return memoryStream;
		}

		public async Task<MemoryStream> GetPersonsExcel()
		{
			MemoryStream memoryStream = new MemoryStream();
			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
				worksheet.Cells["A1"].Value = "Person Name";
				worksheet.Cells["B1"].Value = "Email";
				worksheet.Cells["C1"].Value = "Date Of Birth";
				worksheet.Cells["D1"].Value = "Age";
				worksheet.Cells["E1"].Value = "Gender";
				worksheet.Cells["F1"].Value = "Country";
				worksheet.Cells["G1"].Value = "Address";
				worksheet.Cells["H1"].Value = "Receive News Letters";

				using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
				{
					headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
					headerCells.Style.Font.Bold = true;
				}

				int row = 2;
				List<PersonResponse> persons = _db.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

				foreach(PersonResponse person in persons)
				{
					worksheet.Cells[row,1].Value =person.PersonName ;
					worksheet.Cells[row, 2].Value = person.Email;
					if (person.DateOfBirth.HasValue)
						worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy MM dd");
					worksheet.Cells[row, 4].Value = person.Age;
					worksheet.Cells[row, 5].Value = person.Gender;
					worksheet.Cells[row, 6].Value = person.Country;
					worksheet.Cells[row, 7].Value = person.Address;
					worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;	

					row++;
				}
				worksheet.Cells[$"A1:H{row}"].AutoFitColumns(); 
				await excelPackage.SaveAsync();
			}
			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}
