
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Represent a DTO class that is used as return type of most methods of PersonService
	/// </summary>
	public class PersonResponse
	{
		public Guid PersonID { get; set; }
		public string? PersonName { get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }
		public double? Age { get; set; }
		/// <summary>
		/// Compares the current object data with the parameter object
		/// </summary>
		/// <param name="obj">the PersonRequset object to compare</param>
		/// <returns>True or False, indicating whether all person details are matched with the parameter object </returns>
		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(PersonResponse)) { return false; }
			PersonResponse personResponse = (PersonResponse)obj;

			return
				   personResponse.PersonID == PersonID
				&& personResponse.PersonName == PersonName
				&& personResponse.ReceiveNewsLetters == ReceiveNewsLetters
				&& personResponse.Email == Email
				&& personResponse.DateOfBirth == DateOfBirth
				&& personResponse.Address == Address
				&& personResponse.CountryID == CountryID
				&& personResponse.Gender == Gender;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override string ToString()
		{
			return $"Person Id:{PersonID}, " +
				$"PersonName: {PersonName}, " +
				$"Email: {Email}, " +
				$"DateOfBirth:{DateOfBirth?.ToString("dd MM yyyy")}, " +
				$"Gender:{Gender}, " +
				$"CountryId: {CountryID}, " +
				$"Country: {Country}, " +
				$"Address: {Address}, " +
				$"ReciveNewsLatters: {ReceiveNewsLetters}";
		}

		public PersonUpdateRequest ToPersonUpdateRequest()
		{
			return new PersonUpdateRequest()
			{
				PersonID = PersonID,
				PersonName = PersonName,
				ReceiveNewsLetters = ReceiveNewsLetters,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Address = Address,
				CountryID = CountryID,
				Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
			};
		}
	}

	public static class PersonExtentions
	{
		/// <summary>
		/// Extention method to convert Person to PersonRequest 
		/// </summary>
		/// <param name="person">person object to convert</param>
		/// <returns>returns the converted PersonResponse object</returns>
		public static PersonResponse ToPersonResponse(this Person person)
		{
			return new PersonResponse()
			{
				Gender = person.Gender,
				Address = person.Address,
				Email = person.Email,
				PersonID = person.PersonID,
				PersonName = person.PersonName,
				DateOfBirth = person.DateOfBirth,
				ReceiveNewsLetters = person.ReceiveNewsLetters,
				CountryID = person.CountryID,
				Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
				Country = person.Country?.CountryName
			};
		}
	}
}
