using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Data;

namespace Tuxedo.Storage.Data;

public static class DatabaseSeeder
{
    public static void Seed(TuxedoDbContext db)
    {
        // Ensure the database is created
        db.Database.EnsureCreated();

        // Seed data if the Savings table is empty
        if (!db.Savings.Any())
        {
            db.Savings.AddRange(new[]
            {
                new Saving
                {
                    SavingDate = new DateTime(2025, 1, 1),
                    Description = "Lots",
                    Category = "Billing",
                    Status = "Actual",
                    Amount = 301.00m,
                    Frequency = "One off"
                },
                new Saving
                {
                    SavingDate = new DateTime(2026, 1, 1),
                    Description = "",
                    Category = "Billing",
                    Status = "Forecasted",
                    Amount = 4999.00m,
                    Frequency = "Monthly"
                }
            });

            db.SaveChanges();
        }
    }
}