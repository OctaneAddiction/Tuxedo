using Tuxedo.Storage.Stores;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage;

public static class DatabaseSeeder
{
    public static void Seed(ITuxedoDbContext context)
    {
        // Check if data already exists
        if (!context.CustomerSaving.Any())
        {
            // Seed initial data
            context.CustomerSaving.AddRange(new[]
            {
                new CustomerSaving { Description = "Initial Saving 1", Category = "Billing", Frequency = Shared.Enums.Frequency.OneOff, Status = Shared.Enums.Status.Confirmed, Amount = 100.00m, SavingDate = DateTime.UtcNow },
                new CustomerSaving { Description = "Initial Saving 2", Category = "Filling", Frequency = Shared.Enums.Frequency.Monthly, Status = Shared.Enums.Status.Forecasted, Amount = 200.00m, SavingDate = DateTime.UtcNow }
            });

            context.SaveChangesAsync().Wait();
        }
    }
}