using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Domain.Entities;
using Tuxedo.Shared.Enums;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Tests;

public class ValueTrackerEFTests
{
    [Fact]
    public async Task Can_Add_ValueTracker_To_Database()
    {
        // Arrange: Set up an in-memory database
        var options = new DbContextOptionsBuilder<TuxedoDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new TuxedoDbContext(options);
        var saving = new ValueTracker
        {
            SavingDate = DateTime.UtcNow,
            Description = "Test Saving",
            Category = "Billing",
            Status = Shared.Enums.Status.Confirmed,
            Amount = 100.00m,
            Frequency = Shared.Enums.Frequency.OneOff
        };

        // Act: Add the saving to the database
        context.ValueTracker.Add(saving);
        await context.SaveChangesAsync();

        // Assert: Verify the saving was added
        var savedSaving = await context.ValueTracker.FirstOrDefaultAsync();
        savedSaving.Should().NotBeNull();
        savedSaving.Description.Should().Be("Test Saving");
    }

    [Fact]
    public async Task CreateSeriesAsync_Should_Create_Multiple_Entries_For_Recurring_Savings()
    {
        // Arrange
        var mockDbContext = new Mock<ITuxedoDbContext>();
        var mockValueTrackerDbSet = new Mock<DbSet<ValueTracker>>();

        mockDbContext.Setup(db => db.ValueTracker).Returns(mockValueTrackerDbSet.Object);

        var service = new ValueTrackerCreateService(mockDbContext.Object);

        var request = new ValueTrackerCreateSeriesRequest
        {
            Description = "Monthly cost reduction",
            Frequency = Frequency.Monthly,
            Amount = 1000m,
            SavingDate = new DateTime(2024, 6, 1),
            EndCondition = EndCondition.EndDate,
            EndDate = new DateTime(2024, 12, 31),
            Status = Status.Forecasted,
            Category = "Billing",
            CompanyId = Guid.NewGuid()
        };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        mockValueTrackerDbSet.Verify(
            dbSet => dbSet.AddRangeAsync(It.Is<List<ValueTracker>>(savings =>
                savings.Count == 7 &&
                savings.All(s => s.Description == "Monthly cost reduction") &&
                savings.All(s => s.Amount == 1000m) &&
                savings.All(s => s.Category == "Billing") &&
                savings.All(s => s.Status == Status.Forecasted) &&
                savings.Select(s => s.SavingDate).OrderBy(d => d).SequenceEqual(new[]
                {
                    new DateTime(2024, 6, 1),
                    new DateTime(2024, 7, 1),
                    new DateTime(2024, 8, 1),
                    new DateTime(2024, 9, 1),
                    new DateTime(2024, 10, 1),
                    new DateTime(2024, 11, 1),
                    new DateTime(2024, 12, 1)
                })
            ), It.IsAny<CancellationToken>()),
            Times.Once
        );

        response.SeriesId.Should().NotBeEmpty();
    }
}