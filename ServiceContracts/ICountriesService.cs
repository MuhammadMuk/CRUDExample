using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
	/// <summary>
	/// Represent business logic for manipulating Country entity
	/// </summary>
	public interface ICountriesService
	{
		/// <summary>
		/// Adds a country object to list of countries
		/// </summary>
		/// <param name="countryAddRequest"> a country object to be added</param>
		/// <returns>
		/// Returns the country object after adding it (including the new generated ID
		/// </returns>
		Task<CountryResponse> AddCountry(CountryAddRequest countryAddRequest);
		/// <summary>
		/// Returns All Countries
		/// </summary>
		/// <returns> All Countries forom list  as list of CountryResponse</returns>
		Task<List<CountryResponse>> GetAllCountries();
		/// <summary>
		/// Returns Country Object based on given ID
		/// </summary>
		/// <param name="countryID">Country ID(Guid) to search</param>
		/// <returns>Matching Country as CountryResponse object</returns>
		Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

		Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
	}
}
