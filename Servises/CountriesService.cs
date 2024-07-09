using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Servises
{
	public class CountriesService : ICountriesService
	{
		private readonly PersonsDbContext _db;

		public CountriesService(PersonsDbContext personsDbContext)
		{
			_db = personsDbContext;

		}
		public async Task<CountryResponse> AddCountry(CountryAddRequest countryAddRequest)
		{
			if (countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}
			if (countryAddRequest.CountryName == null)
			{
				throw new ArgumentException(nameof(countryAddRequest.CountryName));
			}
			if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
			{
				throw new ArgumentException("Country Name Already Exists");
			}
			Country country = countryAddRequest.ToCountry();
			country.CountryId = Guid.NewGuid();
			_db.Countries.Add(country);
			await _db.SaveChangesAsync();
			return country.ToCountryResponse();
		}

		public async Task<List<CountryResponse>> GetAllCountries()
		{
			return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();	
		}

		public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
		{
			if (countryID == null)
			{
				return null;
			}
			Country? country_from_list = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId==countryID);

			if (country_from_list == null)
			{
				return null;
			}
			return country_from_list.ToCountryResponse();
		}

		public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
		{
			MemoryStream memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);
			int countriesInserted = 0;
			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Countries"];
				int rowCount = excelWorksheet.Dimension.Rows;
				
				for (int row = 2; row <= rowCount; row++)
				{
					string? cellValue = Convert.ToString(excelWorksheet.Cells[row, 1].Value);
					if(!string.IsNullOrEmpty(cellValue))
					{
						string? countryName = cellValue;
						if(_db.Countries.Where(temp => temp.CountryName==countryName).Count() == 0)
						{
							Country country = new Country() { CountryName = countryName };
							_db.Countries.Add(country);
							await _db.SaveChangesAsync();
							countriesInserted++;
						}
					}
				}
			}
			return countriesInserted;
		}
	}
}
