using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Servises;

namespace CRUDTest
{
	public class CountriesServiceTest
	{
		private readonly ICountriesService _countriesService;

		public CountriesServiceTest()
		{
			_countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
		}
		#region AddCountry
		[Fact]
		public async Task AddCountry_NullCountry()
		{
			//Arrange
			CountryAddRequest request = null;


			//Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				// Act 
				await _countriesService.AddCountry(request);
			});
		}

		[Fact]
		public async Task AddCountry_CountryNameIsNull()
		{
			//Arrange
			CountryAddRequest request = new CountryAddRequest() { CountryName = null };


			//Assert
			await Assert.ThrowsAsync<ArgumentException>(async () =>
			{
				// Act 
			await	_countriesService.AddCountry(request);
			});
		}

		[Fact]
		public async Task AddCountry_DuplicateCountryName()
		{
			//Arrange
			CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };

			//Assert
			await Assert.ThrowsAsync<ArgumentException>(async () =>
			{
				// Act 
			await	_countriesService.AddCountry(request1);
			await	_countriesService.AddCountry(request1);
			});
		}
		[Fact]
		public async Task AddCountry_ProperCountryDetails()
		{
			//Arrange
			CountryAddRequest request = new CountryAddRequest() { CountryName = "Syria" };
			//Act
			CountryResponse response =await _countriesService.AddCountry(request);
			List<CountryResponse> countries_from_GetAllCountries =await _countriesService.GetAllCountries();
			//Assert
			Assert.True(response.CountryId != Guid.Empty);
			Assert.Contains(response, countries_from_GetAllCountries);

		}
		#endregion

		#region GetAllCountries
		[Fact]
		public async Task GetAllCountries_EmptyList()
		{
			//act
			List<CountryResponse> acctual_country_response_list =await _countriesService.GetAllCountries();
			//Assert
			Assert.Empty(acctual_country_response_list);

		}
		[Fact]
		public async Task GetAllCountries_AddFewCountries()
		{
			List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
			{
				new CountryAddRequest() {CountryName = "USA" },
				new CountryAddRequest() {CountryName = "UK" },
				new CountryAddRequest() {CountryName = "Syria" }
			};
			List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

			foreach (CountryAddRequest country in country_request_list)
			{
				countries_list_from_add_country.Add(await _countriesService.AddCountry(country));
			}
			List<CountryResponse> acctualCountryResponseList = await _countriesService.GetAllCountries();

			foreach(CountryResponse expected_country in countries_list_from_add_country)
			{
				Assert.Contains(expected_country, acctualCountryResponseList);
			}

		}
		#endregion

		#region GetCountryByCountryID
		[Fact]
		public async Task GetCountryByCountryID_NullCountryID()
		{
			//arrange
			 Guid? countryID = null;

			// Act
			CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);

			// Assert
			Assert.Null(country_response_from_get_method);
		}
		[Fact]
		public async Task GetCountryByCountryID_ValidCountryID()
		{
			// Arrange
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "Syria" };
			CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

			//Act
			CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryId);

			//Assert
			Assert.Equal(country_response_from_add, country_response_from_get);
		}
		#endregion
	}
}
