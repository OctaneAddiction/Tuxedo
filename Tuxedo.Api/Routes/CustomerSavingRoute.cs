using Tuxedo.Api.Services;
using Tuxedo.Api.Responses;

namespace Tuxedo.Api.Routes;

public static class CustomerSavingRoute
{
	public static void RegisterCustomerSavingRoutes(this WebApplication app)
	{
		app.MapGet("/api/customersaving", GetCustomerSavingAsync);
		app.MapGet("/api/customersaving/{id:guid}", GetCustomerSavingByIdAsync);
		app.MapPost("/api/customersaving", CreateCustomerSavingAsync);
		app.MapPut("/api/customersaving/{id:guid}", UpdateCustomerSavingAsync);
		app.MapDelete("/api/customersaving/{id:guid}", DeleteCustomerSavingAsync);
	}

	private static async Task<IResult> GetCustomerSavingAsync(CustomerSavingService customerSavingService)
	{
		var response = await customerSavingService.GetCustomerSavingAsync();

		if (response == null || !response.Any())
		{
			return Results.NotFound("No customer savings found.");
		}

		return Results.Ok(response);
	}
	private static async Task<IResult> GetCustomerSavingByIdAsync(Guid id, CustomerSavingService customerSavingService)
	{
		var response = await customerSavingService.GetCustomerSavingByIdAsync(id);

		if (response == null)
		{
			return Results.NotFound($"Customer saving with ID {id} not found.");
		}

		return Results.Ok(response);
	}
	private static async Task<IResult> UpdateCustomerSavingAsync(Guid id, UpdateCustomerSavingRequest updateCustomerSavingRequest, CustomerSavingService customerSavingService)
	{
		await customerSavingService.UpdateCustomerSavingAsync(id, updateCustomerSavingRequest);
		return Results.NoContent();
	}
	private static async Task<IResult> DeleteCustomerSavingAsync(Guid id, CustomerSavingService customerSavingService)
	{
		await customerSavingService.DeleteCustomerSavingAsync(id);
		return Results.NoContent();
	}
	private static async Task<IResult> CreateCustomerSavingAsync(CreateCustomerSavingRequest createCustomerSavingRequest, CustomerSavingService customerSavingService)
	{
		await customerSavingService.CreateCustomerSavingAsync(createCustomerSavingRequest);
		return Results.Created($"/api/customersaving/{createCustomerSavingRequest.ObjectId}", createCustomerSavingRequest);
	}
}