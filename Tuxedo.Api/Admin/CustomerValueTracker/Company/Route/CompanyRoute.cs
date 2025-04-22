using Tuxedo.Api.Admin.CompanyValueTracker.Company.Service;
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Request;

namespace Tuxedo.Api.Admin.CustomerValueTracker.Company.Route;

public static class CompanyRoute
{
	public static void RegisterCompanyRoutes(this WebApplication app)
	{
		app.MapGet("/api/company", GetCompaniesAsync);
		app.MapGet("/api/company/{id:guid}", GetCompanyByIdAsync);
		app.MapPost("/api/company", CreateCompanyAsync);
		app.MapPut("/api/company/{id:guid}", UpdateCompanyAsync);
		app.MapDelete("/api/company/{id:guid}", DeleteCompanyAsync);
	}

	private static async Task<IResult> GetCompaniesAsync(CompanyService companyService)
	{
		var response = await companyService.GetCompaniesAsync();

		if (response == null || !response.Any())
		{
			return Results.NotFound("No companies found.");
		}

		return Results.Ok(response);
	}

	private static async Task<IResult> GetCompanyByIdAsync(Guid id, CompanyService companyService)
	{
		var response = await companyService.GetCompanyByIdAsync(id);

		if (response == null)
		{
			return Results.NotFound($"Company with ID {id} not found.");
		}

		return Results.Ok(response);
	}

	private static async Task<IResult> CreateCompanyAsync(CreateCompanyRequest createCompanyRequest, CompanyService companyService)
	{
		await companyService.CreateCompanyAsync(createCompanyRequest);
		return Results.Created($"/api/company/{createCompanyRequest.Name}", createCompanyRequest);
	}

	private static async Task<IResult> UpdateCompanyAsync(Guid id, UpdateCompanyRequest updateCompanyRequest, CompanyService companyService)
	{
		await companyService.UpdateCompanyAsync(id, updateCompanyRequest);
		return Results.NoContent();
	}

	private static async Task<IResult> DeleteCompanyAsync(Guid id, CompanyService companyService)
	{
		await companyService.DeleteCompanyAsync(id);
		return Results.NoContent();
	}
}
