using Tuxedo.Api.Services;
using Tuxedo.Api.Responses;

namespace Tuxedo.Api.Routes;

public static class CustomerRoute
{
    public static void RegisterCustomerRoutes(this WebApplication app)
    {
        app.MapGet("/api/customer", GetCustomersAsync);
        app.MapGet("/api/customer/{id:guid}", GetCustomerByIdAsync);
        app.MapPost("/api/customer", CreateCustomerAsync);
        app.MapPut("/api/customer/{id:guid}", UpdateCustomerAsync);
        app.MapDelete("/api/customer/{id:guid}", DeleteCustomerAsync);
    }

    private static async Task<IResult> GetCustomersAsync(CustomerService customerService)
    {
        var response = await customerService.GetCustomersAsync();

        if (response == null || !response.Any())
        {
            return Results.NotFound("No customers found.");
        }

        return Results.Ok(response);
    }

    private static async Task<IResult> GetCustomerByIdAsync(Guid id, CustomerService customerService)
    {
        var response = await customerService.GetCustomerByIdAsync(id);

        if (response == null)
        {
            return Results.NotFound($"Customer with ID {id} not found.");
        }

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest, CustomerService customerService)
    {
        await customerService.CreateCustomerAsync(createCustomerRequest);
        return Results.Created($"/api/customer/{createCustomerRequest.Name}", createCustomerRequest);
    }

    private static async Task<IResult> UpdateCustomerAsync(Guid id, UpdateCustomerRequest updateCustomerRequest, CustomerService customerService)
    {
        await customerService.UpdateCustomerAsync(id, updateCustomerRequest);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteCustomerAsync(Guid id, CustomerService customerService)
    {
        await customerService.DeleteCustomerAsync(id);
        return Results.NoContent();
    }
}
