using Tuxedo.Storage.Stores;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage;

public static class DatabaseSeeder
{
	public static void Seed(ITuxedoDbContext context)
	{
		// Check if data already exists
		if (!context.Company.Any())
		{
			// Seed initial data
			var company1 = new Company { Name = "Company 1" };
			var company2 = new Company { Name = "Company 2" };

			context.Company.AddRange(company1, company2);

			context.CompanySaving.AddRange(new[]
			{
					new CompanySaving { Description = "Initial Saving 1", Category = "Billing", Frequency = Shared.Enums.Frequency.OneOff, Status = Shared.Enums.Status.Confirmed, Amount = 100.00m, SavingDate = DateTime.UtcNow, Company = company1 },
					new CompanySaving { Description = "Initial Saving 2", Category = "Filling", Frequency = Shared.Enums.Frequency.Monthly, Status = Shared.Enums.Status.Forecasted, Amount = 200.00m, SavingDate = DateTime.UtcNow, Company = company2 }
				});

			context.SaveChangesAsync().Wait();
		}
	}
}
