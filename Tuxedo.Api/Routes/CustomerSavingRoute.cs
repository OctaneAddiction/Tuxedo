using Tuxedo.Api.Services;
using Tuxedo.Api.Responses;
using Tuxedo.Api.Dtos; // Assuming this is where CustomerSavingDto and GetCustomerSavingResponse are defined.

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
        var customerSavings = await customerSavingService.GetCustomerSavingAsync();

        // Map CustomerSavingDto to GetCustomerSavingResponse
        var response = customerSavings.Select(saving => new GetCustomerSavingResponse
        {
            ObjectId = saving.ObjectId,
            Description = saving.Description,
            Amount = saving.Amount,
            Status = saving.Status,
            SavingDate = saving.SavingDate,
            Category = saving.Category,
            Frequency = saving.Frequency
        }).ToList();

        return Results.Ok(response);
    }
    private static async Task<IResult> GetCustomerSavingByIdAsync(Guid id, CustomerSavingService customerSavingService)
    {
        var customerSaving = await customerSavingService.GetCustomerSavingByIdAsync(id);

        // Map CustomerSavingDto to GetCustomerSavingResponse
        var response = new GetCustomerSavingResponse
        {
            ObjectId = customerSaving.ObjectId,
            Description = customerSaving.Description,
            Amount = customerSaving.Amount,
            Status = customerSaving.Status,
            SavingDate = customerSaving.SavingDate,
            Category = customerSaving.Category,
            Frequency = customerSaving.Frequency
        };

        return Results.Ok(response);
    }
    private static async Task<IResult> UpdateCustomerSavingAsync(Guid id, CustomerSavingDto customerSavingDto, CustomerSavingService customerSavingService)
    {
        await customerSavingService.UpdateCustomerSavingAsync(id, customerSavingDto);
        return Results.NoContent();
    }
    private static async Task<IResult> DeleteCustomerSavingAsync(Guid id, CustomerSavingService customerSavingService)
    {
        await customerSavingService.DeleteCustomerSavingAsync(id);
        return Results.NoContent();
    }
    private static async Task<IResult> CreateCustomerSavingAsync(CustomerSavingDto customerSavingDto, CustomerSavingService customerSavingService)
    {
        await customerSavingService.CreateCustomerSavingAsync(customerSavingDto);
        return Results.Created($"/api/customersaving/{customerSavingDto.ObjectId}", customerSavingDto);
    }
}