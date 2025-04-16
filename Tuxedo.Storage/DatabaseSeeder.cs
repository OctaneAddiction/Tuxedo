using Tuxedo.Storage.Stores;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage;

public static class DatabaseSeeder
{
	public static void Seed(ITuxedoDbContext context)
	{
		// Check if data already exists
		if (!context.Customer.Any())
		{
			// Seed initial data
			var customer1 = new Customer { Name = "Customer 1" };
			var customer2 = new Customer { Name = "Customer 2" };

			context.Customer.AddRange(customer1, customer2);

			context.CustomerSaving.AddRange(new[]
			{
					new CustomerSaving { Description = "Initial Saving 1", Category = "Billing", Frequency = Shared.Enums.Frequency.OneOff, Status = Shared.Enums.Status.Confirmed, Amount = 100.00m, SavingDate = DateTime.UtcNow, Customer = customer1 },
					new CustomerSaving { Description = "Initial Saving 2", Category = "Filling", Frequency = Shared.Enums.Frequency.Monthly, Status = Shared.Enums.Status.Forecasted, Amount = 200.00m, SavingDate = DateTime.UtcNow, Customer = customer2 }
				});

			context.SaveChangesAsync().Wait();
		}
	}
}
