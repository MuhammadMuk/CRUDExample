using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	public class PersonUpdateRequest
	{
		[Required(ErrorMessage = "Person ID Can not be empty")]
		public Guid PersonID { get; set; }
		[Required(ErrorMessage = "Person Name Can not be empty")]
		public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email Can not be empty")]
		[EmailAddress(ErrorMessage = "Email should be a valid email")]
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public GenderOptions? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }
		/// <summary>
		/// Convert a current object of PersonAddRequest to a new object of Person Type.
		/// </summary>
		/// <returns></returns>
		public Person ToPerson()
		{
			return new Person()
			{
				PersonID = PersonID, 
				Address = Address,
				DateOfBirth = DateOfBirth,
				Email = Email,
				Gender = Gender.ToString(),
				CountryID = CountryID,
				PersonName = PersonName,
				ReceiveNewsLetters = ReceiveNewsLetters
			};
		}
	}
}
