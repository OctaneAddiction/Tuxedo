using Tuxedo.Api.Services;

namespace Tuxedo.Api.Routes;

public static class CustomerSavingRoute
{
    public static void RegisterCustomerSavingRoutes(this WebApplication app)
	{
		app.MapGet("/api/customersaving", GetCustomerSavingAwait);
	}

    private static async Task<IResult> GetCustomerSavingAwait(CustomerSavingService customerSavingService)
    {
        var customerSavings = await customerSavingService.GetCustomerSavingAsync();
        return Results.Ok(customerSavings);
    }
   

        //app.MapPost("/api/savings", async (TuxedoDbContext db, CustomerSaving saving) =>
        //{
        //    db.Savings.Add(saving);
        //    await db.SaveChangesAsync();
        //    return Results.Created($"/api/savings/{saving.Id}", saving);
        //});
    
}