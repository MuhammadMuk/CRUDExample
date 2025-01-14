using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Servises;

namespace CRUDExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<ICountriesService, CountriesService>();
			builder.Services.AddScoped<IPersonsService, PersonsService>();

			builder.Services.AddDbContext<PersonsDbContext>(
				options =>{ options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); }
				);
			//Data Source=(localdb)\ProjectModels;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
			var app = builder.Build();

			if (builder.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
			app.UseStaticFiles();
			app.UseRouting();
			app.MapControllers();

			app.Run();
		}
	}
}
