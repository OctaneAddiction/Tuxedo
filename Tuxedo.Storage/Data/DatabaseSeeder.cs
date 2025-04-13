
using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Storage.Data;

public static class DatabaseSeeder
{
    public static void Seed(TuxedoDbContext db)
    {
        // Ensure the database is created
        db.Database.EnsureCreated();

        // Seed data if the Savings table is empty
        if (!db.CustomerSaving.Any())
        {
            db.CustomerSaving.AddRange(new[]
            {
                new CustomerSaving
                {
                    SavingDate = new DateTime(2025, 1, 1),
                    Description = "Lots",
                    Category = "Billing",
                    Status = Shared.Enums.Status.Confirmed,
                    Amount = 301.00m,
                    Frequency = Shared.Enums.Frequency.OneOff
                },
                new CustomerSaving
                {
                    SavingDate = new DateTime(2026, 1, 1),
                    Description = "",
                    Category = "Billing",
                    Status = Shared.Enums.Status.Forecasted,
                    Amount = 4999.00m,
                    Frequency =  Shared.Enums.Frequency.Monthly
                }
            });

            db.SaveChanges();
        }
    }
}