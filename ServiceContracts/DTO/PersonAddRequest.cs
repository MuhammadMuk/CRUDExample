
using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Acts as DTO for insearting new person	
	/// </summary>
	public class PersonAddRequest
	{
		[Required(ErrorMessage ="Person Name Can not be empty")]
		public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email Can not be empty")]
		[EmailAddress(ErrorMessage ="Email should be a valid email")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }
		[Required(ErrorMessage ="Gender can not be blank")]
		public GenderOptions? Gender { get; set; }
		[Required(ErrorMessage ="Please Select country")]
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
