using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Tests;

public class CustomerSavingTests
{
    [Fact]
    public async Task Can_Add_Saving_To_Database()
    {
        // Arrange: Set up an in-memory database
        var options = new DbContextOptionsBuilder<TuxedoDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new TuxedoDbContext(options);
        var saving = new CompanySaving
        {
            SavingDate = DateTime.UtcNow,
            Description = "Test Saving",
            Category = "Billing",
            Status =  Shared.Enums.Status.Confirmed,
            Amount = 100.00m,
            Frequency = Shared.Enums.Frequency.OneOff
        };

        // Act: Add the saving to the database
        context.CompanySaving.Add(saving);
        await context.SaveChangesAsync();

        // Assert: Verify the saving was added
        var savedSaving = await context.CompanySaving.FirstOrDefaultAsync();
        savedSaving.Should().NotBeNull();
        savedSaving.Description.Should().Be("Test Saving");
    }
}