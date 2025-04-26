using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tuxedo.Api.Admin.ValueTracker.Get;
using Tuxedo.Domain.Entities;
using Tuxedo.Shared.Enums;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Tests;

public class ValueTrackerGetServiceTests
{
    [Fact]
    public async Task GetByCompanyIdAsync_Should_Calculate_Totals_Correctly()
    {
        // Arrange
        var mockDbContext = new Mock<ITuxedoDbContext>();
        var mockValueTrackerDbSet = new Mock<DbSet<ValueTracker>>();

        var companyId = Guid.NewGuid();
        var valueTrackers = new List<ValueTracker>
        {
            new ValueTracker { Id = Guid.NewGuid(), CompanyId = companyId, Amount = 100m, Status = Status.Forecasted, Description = "Tracker 1", Category = "Category 1", SavingDate = DateTime.UtcNow, Frequency = Frequency.OneOff },
            new ValueTracker { Id = Guid.NewGuid(), CompanyId = companyId, Amount = 200m, Status = Status.Forecasted, Description = "Tracker 2", Category = "Category 2", SavingDate = DateTime.UtcNow, Frequency = Frequency.Monthly },
            new ValueTracker { Id = Guid.NewGuid(), CompanyId = companyId, Amount = 150m, Status = Status.Confirmed, Description = "Tracker 3", Category = "Category 3", SavingDate = DateTime.UtcNow, Frequency = Frequency.Quarterly },
            new ValueTracker { Id = Guid.NewGuid(), CompanyId = companyId, Amount = 50m, Status = Status.Confirmed, Description = "Tracker 4", Category = "Category 4", SavingDate = DateTime.UtcNow, Frequency = Frequency.Annual }
        };

        var asyncValueTrackers = new TestAsyncEnumerable<ValueTracker>(valueTrackers);

        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.Provider).Returns(asyncValueTrackers.Provider);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.Expression).Returns(asyncValueTrackers.Expression);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.ElementType).Returns(asyncValueTrackers.ElementType);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.GetEnumerator()).Returns(asyncValueTrackers.GetEnumerator());
        mockValueTrackerDbSet.As<IAsyncEnumerable<ValueTracker>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(asyncValueTrackers.GetAsyncEnumerator());

        mockDbContext.Setup(db => db.ValueTracker).Returns(mockValueTrackerDbSet.Object);

        var service = new ValueTrackerGetService(mockDbContext.Object);

        // Act
        var result = await service.GetByCompanyIdAsync(companyId, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1); // Verify a single response is returned
        var response = result.First();

        // Verify the totals
        response.TotalEstimatedAmountSaved.Should().Be(300m); // 100 + 200
        response.TotalActualAmountSpent.Should().Be(200m); // 150 + 50
        response.TotalCount.Should().Be(4); // 4 entries in total

        // Verify the list of ValueTrackers
        response.ValueTrackers.Should().HaveCount(4);
        response.ValueTrackers.Should().Contain(v => v.Description == "Tracker 1" && v.Amount == 100m && v.Status == Status.Forecasted);
        response.ValueTrackers.Should().Contain(v => v.Description == "Tracker 2" && v.Amount == 200m && v.Status == Status.Forecasted);
        response.ValueTrackers.Should().Contain(v => v.Description == "Tracker 3" && v.Amount == 150m && v.Status == Status.Confirmed);
        response.ValueTrackers.Should().Contain(v => v.Description == "Tracker 4" && v.Amount == 50m && v.Status == Status.Confirmed);
    }
}
