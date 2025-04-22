using Tuxedo.Api.Admin.CompanyValueTracker.CompanySaving.Service;
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Request;

namespace Tuxedo.Api.Admin.CompanyValueTracker.CompanySaving.Route;

public static class CompanySavingRoute
{
	public static void RegisterCompanySavingRoutes(this WebApplication app)
	{
		app.MapGet("/api/companysaving", GetCompanySavingAsync);
		app.MapGet("/api/companysaving/{id:guid}", GetCompanySavingByIdAsync);
		app.MapPost("/api/companysaving", CreateCompanySavingAsync);
		app.MapPut("/api/companysaving/{id:guid}", UpdateCompanySavingAsync);
		app.MapDelete("/api/companysaving/{id:guid}", DeleteCompanySavingAsync);
	}

	private static async Task<IResult> GetCompanySavingAsync(CompanySavingService companySavingService)
	{
		var response = await companySavingService.GetCompanySavingAsync();

		if (response == null || !response.Any())
		{
			return Results.NotFound("No company savings found.");
		}

		return Results.Ok(response);
	}
	private static async Task<IResult> GetCompanySavingByIdAsync(Guid id, CompanySavingService companySavingService)
	{
		var response = await companySavingService.GetCompanySavingByIdAsync(id);

		if (response == null)
		{
			return Results.NotFound($"Company saving with ID {id} not found.");
		}

		return Results.Ok(response);
	}
	private static async Task<IResult> UpdateCompanySavingAsync(Guid id, UpdateCompanySavingRequest updateCompanySavingRequest, CompanySavingService companySavingService)
	{
		await companySavingService.UpdateCompanySavingAsync(id, updateCompanySavingRequest);
		return Results.NoContent();
	}
	private static async Task<IResult> DeleteCompanySavingAsync(Guid id, CompanySavingService companySavingService)
	{
		await companySavingService.DeleteCompanySavingAsync(id);
		return Results.NoContent();
	}
	private static async Task<IResult> CreateCompanySavingAsync(CreateCompanySavingRequest createCompanySavingRequest, CompanySavingService companySavingService)
	{
		await companySavingService.CreateCompanySavingAsync(createCompanySavingRequest);
		return Results.Created($"/api/companysaving/{createCompanySavingRequest.Id}", createCompanySavingRequest);
	}
}