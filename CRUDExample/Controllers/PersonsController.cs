using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
	[Route("[controller]")]
	public class PersonsController : Controller
	{
		private readonly IPersonsService _personsService;
		private readonly ICountriesService _countriesService;

		public PersonsController(ICountriesService countriesService, IPersonsService personsService)
		{
			_countriesService = countriesService;
			_personsService = personsService;
		}
		[Route("/")]
		[Route("[action]")]

		public async Task<IActionResult> Index(string searchBy, string? searchString, 
								   string sortBy = nameof(PersonResponse.PersonName), 
								   SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			ViewBag.SearchFields = new Dictionary<string, string>()
			{
				{ nameof(PersonResponse.PersonName), "Person Name" },
				{ nameof(PersonResponse.Email), "Email" },
				{ nameof(PersonResponse.DateOfBirth), "DateOfBirth" },
				{ nameof(PersonResponse.Gender), "Gender" },
				{ nameof(PersonResponse.CountryID), "Country" },
				{ nameof(PersonResponse.Address), "Address" },
			};
			List<PersonResponse> personResponses =await _personsService.GetFilterdPersons(searchBy, searchString);

			ViewBag.CurrentSearchBy = searchBy;
			ViewBag.CurrentSearchString = searchString;

			//sort
			List<PersonResponse> sortedPersons =await _personsService.GetSortedPersons(personResponses,sortBy, sortOrder);
			ViewBag.CurrentSortBy = sortBy;
			ViewBag.CurrentSortOrder = sortOrder.ToString();
			return View(sortedPersons);
		}

		[Route("[action]")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			List<CountryResponse> countries =await _countriesService.GetAllCountries();	
			ViewBag.Countries = countries.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryId.ToString() });
			return View();
		}
		[Route("[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
		{
			if (!ModelState.IsValid)
			{
				List<CountryResponse> countries =await _countriesService.GetAllCountries();
				ViewBag.Countries = countries.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryId.ToString() });

				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() ;
				return View();
			}
			await _personsService.AddPerson(personAddRequest);
			return RedirectToAction("Index","Persons");
		}
		[HttpGet]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Edit(Guid personID)
		{
			PersonResponse? personResponse =await _personsService.GetPersonByPersonID(personID);
			if (personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
			List<CountryResponse> countries =await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryId.ToString() });

			return View(personUpdateRequest);
		}
		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse =await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
			if (personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			if (ModelState.IsValid)
			{
				PersonResponse updatedPerson =await _personsService.UpdatePerson(personUpdateRequest);
				return RedirectToAction("Index", "Persons");
			}
			else
			{
				List<CountryResponse> countries =await _countriesService.GetAllCountries();
				ViewBag.Countries = countries.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryId.ToString() });

				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View(personResponse.ToPersonUpdateRequest());
			}
		}

		[HttpGet]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(Guid? personID)
		{
			PersonResponse? personResponse =await _personsService.GetPersonByPersonID(personID);
			if (personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			return View(personResponse);
		}

		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse =await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
			if (personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			await _personsService.DeletePerson(personResponse.PersonID);
			return RedirectToAction("Index", "Persons");
		}
		[Route("[action]")]
		public async Task<IActionResult> PersonsPDF()
		{
			List<PersonResponse> personResponses =await _personsService.GetAllPersons();
			return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins()
				{
					Top = 20,
					Bottom = 20,
					Left = 20,
					Right = 20
				},
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
			};
		}
		[Route("[action]")]
		public async Task<IActionResult> PersonsCSV()
		{
			MemoryStream stream =await _personsService.GetPersonsCSV();
			return File(stream,"application/octet-stream","person.csv");
		}
		[Route("[action]")]
		public async Task<IActionResult> PersonsExcel()
		{
			MemoryStream stream = await _personsService.GetPersonsExcel();
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "person.xlsx");
		}
	}
}
