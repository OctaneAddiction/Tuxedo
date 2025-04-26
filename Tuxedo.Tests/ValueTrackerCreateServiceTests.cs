using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Stores;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Shared.Enums;

namespace Tuxedo.Tests;

public class ValueTrackerCreateServiceTests
{
    [Fact]
    public async Task CreateSeriesAsync_Should_Create_Multiple_Entries_For_Recurring_Savings()
    {
        // Arrange
        var mockDbContext = new Mock<ITuxedoDbContext>();
        var mockValueTrackerDbSet = new Mock<DbSet<ValueTracker>>();

        // Setup the mock DbSet to simulate adding entries
        var valueTrackerList = new List<ValueTracker>();
        mockValueTrackerDbSet.Setup(m => 
            m.AddRangeAsync(It.IsAny<IEnumerable<ValueTracker>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<ValueTracker>, CancellationToken>((entries, _) => valueTrackerList.AddRange(entries));

        mockDbContext.Setup(db => db.ValueTracker).Returns(mockValueTrackerDbSet.Object);

        var service = new ValueTrackerCreateService(mockDbContext.Object);

        var request = new ValueTrackerCreateSeriesRequest { Description = "Monthly cost reduction", Frequency = Shared.Enums.Frequency.Monthly, Amount = 1000m, SavingDate = new DateTime(2024, 6, 1), EndCondition = Shared.Enums.EndCondition.EndDate, EndDate = new DateTime(2024, 12, 31), Status = Shared.Enums.Status.Forecasted, Category = "Billing", CompanyId = Guid.NewGuid() };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        valueTrackerList.Should().HaveCount(7); // Verify 7 entries were created
        valueTrackerList.All(s => s.Description == "Monthly cost reduction").Should().BeTrue();
        valueTrackerList.All(s => s.Amount == 1000m).Should().BeTrue();
        valueTrackerList.All(s => s.Category == "Billing").Should().BeTrue();
        valueTrackerList.All(s => s.Status == Status.Forecasted).Should().BeTrue();
        valueTrackerList.Select(s => s.SavingDate).OrderBy(d => d).Should().BeEquivalentTo(new[]
        {
            new DateTime(2024, 6, 1),
            new DateTime(2024, 7, 1),
            new DateTime(2024, 8, 1),
            new DateTime(2024, 9, 1),
            new DateTime(2024, 10, 1),
            new DateTime(2024, 11, 1),
            new DateTime(2024, 12, 1)
        });
        response.SeriesId.Should().NotBeEmpty(); // Verify the series ID is returned
    }
}