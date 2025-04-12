using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Data;
using Xunit;

namespace Tuxedo.Tests.Tests;

public class SavingTests
{
    [Fact]
    public async Task Can_Add_Saving_To_Database()
    {
        // Arrange: Set up an in-memory database
        var options = new DbContextOptionsBuilder<TuxedoDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new TuxedoDbContext(options);
        var saving = new Saving
        {
            SavingDate = DateTime.UtcNow,
            Description = "Test Saving",
            Category = "Billing",
            Status = "Actual",
            Amount = 100.00m,
            Frequency = "One off"
        };

        // Act: Add the saving to the database
        context.Savings.Add(saving);
        await context.SaveChangesAsync();

        // Assert: Verify the saving was added
        var savedSaving = await context.Savings.FirstOrDefaultAsync();
        savedSaving.Should().NotBeNull();
        savedSaving.Description.Should().Be("Test Saving");
    }
}