
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Servises;
using Xunit.Abstractions;

namespace CRUDTest
{
	public class PersonServiceTest
	{
		private readonly IPersonsService _personService;
		private readonly ICountriesService _countriesService;
		private readonly ITestOutputHelper _testOutputHelper;

		public PersonServiceTest(ITestOutputHelper testOutputHelper)
		{
			_countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
			_personService = new PersonsService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
			_testOutputHelper = testOutputHelper;
		}
		#region AddPerson
		[Fact]
		public async Task AddPerson_NullPerson()
		{
			PersonAddRequest personAddRequest = null;

			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
			await	_personService.AddPerson(personAddRequest);
			});
		}

		[Fact]
		public async Task AddPerson_PersonNameIsNull()
		{
			PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = null };

			await Assert.ThrowsAsync<ArgumentException>(async () =>
			{
				await _personService.AddPerson(personAddRequest);
			});
		}

		[Fact]
		public async Task AddPerson_ProperPersonDetails()
		{
			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@sass",
				CountryID = Guid.NewGuid(),
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};

			PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);
			List<PersonResponse> persons_list = await _personService.GetAllPersons();

			Assert.True(person_response_from_add.PersonID != Guid.Empty);
			Assert.Contains(person_response_from_add, persons_list);
		}
		#endregion
		#region GetPersonByPersonID
		[Fact]
		public async Task GetPersonByPersonID_NullPersonID()
		{
			Guid? personID = null;
			PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

			Assert.Null(person_response_from_get);
		}
		[Fact]
		public async Task GetPersonByPersonID_WithPersonID()
		{
			CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Syira" };
			CountryResponse country_response = await _countriesService.AddCountry(country_request);
			PersonAddRequest person_request = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@gmail.com",
				CountryID = country_response.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};

			PersonResponse person_response_from_add = await _personService.AddPerson(person_request);
			PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

			Assert.Equal(person_response_from_add, person_response_from_get);
		}
		#endregion
		#region GetAllPersons
		[Fact]
		public async Task GetAllPersons_EmptyList()
		{
			List<PersonResponse> person_from_get = await _personService.GetAllPersons();

			Assert.Empty(person_from_get);
		}
		[Fact]
		public async Task GetAllPersons_AddFewPersons()
		{
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Syria" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "UAE" };

			CountryResponse country_response_from_add_1 = await _countriesService.AddCountry(country_request_1);
			CountryResponse country_response_from_add_2 = await _countriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_2 = new PersonAddRequest()
			{
				PersonName = "Muhammad1",
				Address = "addres",
				Email = "asdasd1@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_3 = new PersonAddRequest()
			{
				PersonName = "Muhammad2",
				Address = "addres",
				Email = "asdasd2@qweqw",
				CountryID = country_response_from_add_2.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = false,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
			List<PersonResponse> person_respons_list_from_add = new List<PersonResponse>();
			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _personService.AddPerson(person_request);
				person_respons_list_from_add.Add(person_response);
			}
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				_testOutputHelper.WriteLine(person_response_from_add.ToString());
			}
			List<PersonResponse> person_list_from_get = await _personService.GetAllPersons();
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse person_response_from_get in person_list_from_get)
			{
				_testOutputHelper.WriteLine(person_response_from_get.ToString());
			}

			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				Assert.Contains(person_response_from_add, person_list_from_get);
			}

		}
		#endregion

		#region GetFilterdPerson
		[Fact]
		public async Task GetFilterdPerson_EmptySearchText()
		{
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Syria" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "UAE" };

			CountryResponse country_response_from_add_1 = await _countriesService.AddCountry(country_request_1);
			CountryResponse country_response_from_add_2 = await _countriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_2 = new PersonAddRequest()
			{
				PersonName = "Muhammad1",
				Address = "addres",
				Email = "asdasd1@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_3 = new PersonAddRequest()
			{
				PersonName = "Muhammad2",
				Address = "addres",
				Email = "asdasd2@qweqw",
				CountryID = country_response_from_add_2.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = false,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
			List<PersonResponse> person_respons_list_from_add = new List<PersonResponse>();
			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _personService.AddPerson(person_request);
				person_respons_list_from_add.Add(person_response);
			}
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				_testOutputHelper.WriteLine(person_response_from_add.ToString());
			}
			List<PersonResponse> person_list_from_search = await _personService.GetFilterdPersons(nameof(Person.PersonName), "");
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse person_response_from_get in person_list_from_search)
			{
				_testOutputHelper.WriteLine(person_response_from_get.ToString());
			}

			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				Assert.Contains(person_response_from_add, person_list_from_search);
			}

		}
		[Fact]
		public async Task GetFilterdPerson_SearchByPersonName()
		{
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Syria" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "UAE" };

			CountryResponse country_response_from_add_1 = await _countriesService.AddCountry(country_request_1);
			CountryResponse country_response_from_add_2 = await _countriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_2 = new PersonAddRequest()
			{
				PersonName = "Muhammad1",
				Address = "addres",
				Email = "asdasd1@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_3 = new PersonAddRequest()
			{
				PersonName = "Nader",
				Address = "addres",
				Email = "asdasd2@qweqw",
				CountryID = country_response_from_add_2.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = false,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
			List<PersonResponse> person_respons_list_from_add = new List<PersonResponse>();
			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _personService.AddPerson(person_request);
				person_respons_list_from_add.Add(person_response);
			}
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				_testOutputHelper.WriteLine(person_response_from_add.ToString());
			}
			List<PersonResponse> person_list_from_search = await _personService.GetFilterdPersons(nameof(Person.PersonName), "ma");
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse person_response_from_get in person_list_from_search)
			{
				_testOutputHelper.WriteLine(person_response_from_get.ToString());
			}

			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				if (person_response_from_add.PersonName != null && person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
				{
					Assert.Contains(person_response_from_add, person_list_from_search);
				}

			}

		}
		#endregion

		#region GetSortedPerson
		[Fact]
		public async Task GetSortedPerson()
		{
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Syria" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "UAE" };

			CountryResponse country_response_from_add_1 = await _countriesService.AddCountry(country_request_1);
			CountryResponse country_response_from_add_2 = await _countriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_2 = new PersonAddRequest()
			{
				PersonName = "Suhammad1",
				Address = "addres",
				Email = "asdasd1@qweqw",
				CountryID = country_response_from_add_1.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			PersonAddRequest person_request_3 = new PersonAddRequest()
			{
				PersonName = "Nader",
				Address = "addres",
				Email = "asdasd2@qweqw",
				CountryID = country_response_from_add_2.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = false,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};
			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
			List<PersonResponse> person_respons_list_from_add = new List<PersonResponse>();
			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _personService.AddPerson(person_request);
				person_respons_list_from_add.Add(person_response);
			}
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse person_response_from_add in person_respons_list_from_add)
			{
				_testOutputHelper.WriteLine(person_response_from_add.ToString());
			}
			List<PersonResponse> all_persons = await _personService.GetAllPersons();
			List<PersonResponse> person_list_from_sort = await _personService.GetSortedPersons(all_persons, nameof(Person.PersonName), SortOrderOptions.DESC);
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse person_response_from_get in person_list_from_sort)
			{
				_testOutputHelper.WriteLine(person_response_from_get.ToString());
			}
			person_respons_list_from_add = person_respons_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();
			for (int i = 0; i < person_respons_list_from_add.Count; i++)
			{
				Assert.Equal(person_respons_list_from_add[i], person_list_from_sort[i]);
			}

		}
		#endregion

		#region UpdatePerson
		[Fact]
		public async Task UpdatePerson_NullPerson()
		{
			PersonUpdateRequest? person_update = null;
			await Assert.ThrowsAsync<ArgumentNullException>(async() =>
			{
				PersonResponse person_request_from_add = await _personService.UpdatePerson(person_update);
			});
		}
		[Fact]
		public async Task UpdatePerson_InValidPersonID()
		{
			PersonUpdateRequest? person_update = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };
			await Assert.ThrowsAsync<ArgumentException>(async() =>
			{
				PersonResponse person_request_from_add = await _personService.UpdatePerson(person_update);
			});
		}
		[Fact]
		public async Task UpdatePerson_PersonNameIsNull()
		{
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName="UK" };
			CountryResponse countrey_response_from_add = await _countriesService.AddCountry(country_add_request);
			PersonAddRequest person_add_request = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@sass",
				CountryID = countrey_response_from_add.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};

			PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);
			PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
			person_update_request.PersonName = null; 
			await Assert.ThrowsAsync<ArgumentException>(async() =>
			{
				 PersonResponse person_request_from_add = await _personService.UpdatePerson(person_update_request);
			});
		}
		[Fact]
		public async Task UpdatePerson_PersonFullDetailsUpdation()
		{
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
			CountryResponse countrey_response_from_add = await _countriesService.AddCountry(country_add_request);
			PersonAddRequest person_add_request = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@sass",
				CountryID = countrey_response_from_add.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};

			PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);
			PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
			person_update_request.PersonName = "Zain";
			person_update_request.Email = "Zain@zain.com";
			PersonResponse person_request_from_update = await _personService.UpdatePerson(person_update_request);

			PersonResponse? person_request_from_get = await _personService.GetPersonByPersonID(person_request_from_update.PersonID);

			Assert.Equal(person_request_from_get,person_request_from_update);
		}
		#endregion

		#region DeletePerson
		[Fact]
		public async Task DeletePerson_ValidPersonID()
		{
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
			CountryResponse countrey_response_from_add = await _countriesService.AddCountry(country_add_request);
			PersonAddRequest person_add_request = new PersonAddRequest()
			{
				PersonName = "Muhammad",
				Address = "addres",
				Email = "asdasd@sass",
				CountryID = countrey_response_from_add.CountryId,
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true,
				DateOfBirth = DateTime.Parse("1998-01-01")
			};

			PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

			bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);
			Assert.True(isDeleted);
		}
		[Fact]
		public async Task DeletePerson_InvalidPersonID()
		{
			bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());
			Assert.False(isDeleted);
		}
		#endregion
	}
}
