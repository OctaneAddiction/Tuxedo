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
    //[Fact]
    public async Task GetByCompanyIdAsync_Returns_ValueTrackers_By_CompanyId()
    {
        // Arrange
        var mockDbContext = new Mock<ITuxedoDbContext>();
        var mockValueTrackerDbSet = new Mock<DbSet<ValueTracker>>();

        var companyId = Guid.NewGuid();
        var valueTrackers = new List<ValueTracker>
        {
            new ValueTracker
            {
                Id = Guid.NewGuid(),
                Description = "Tracker 1",
                Amount = 1000m,
                Category = "Billing",
                SavingDate = new DateTime(2024, 6, 1),
                Status = Status.Forecasted,
                Frequency = Frequency.OneOff,
                CompanyId = companyId
            },
            new ValueTracker
            {
                Id = Guid.NewGuid(),
                Description = "Tracker 2",
                Amount = 2000m,
                Category = "Operations",
                SavingDate = new DateTime(2024, 7, 1),
                Status = Status.Confirmed,
                Frequency = Frequency.Monthly,
                CompanyId = companyId
            }
        }.AsQueryable();

        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.Provider).Returns(valueTrackers.Provider);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.Expression).Returns(valueTrackers.Expression);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.ElementType).Returns(valueTrackers.ElementType);
        mockValueTrackerDbSet.As<IQueryable<ValueTracker>>().Setup(m => m.GetEnumerator()).Returns(valueTrackers.GetEnumerator());

        mockDbContext.Setup(db => db.ValueTracker).Returns(mockValueTrackerDbSet.Object);

        var service = new ValueTrackerGetService(mockDbContext.Object);

        // Act
        var result = await service.GetByCompanyIdAsync(companyId, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var companyResponse = result.First();
        companyResponse.ValueTrackers.Should().HaveCount(2);
        companyResponse.TotalCount.Should().Be(2);
    }
}
