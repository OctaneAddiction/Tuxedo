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
    public async Task CreateSeriesAsync_Create_OneOff_Entry()
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

        var request = new ValueTrackerCreateSeriesRequest { Description = "One-off cost reduction", Frequency = Frequency.OneOff, Amount = 500m, SavingDate = new DateTime(2024, 6, 1), EndCondition = EndCondition.NoEndDate, EndDate = null, Status = Status.Forecasted, Category = "Billing", CompanyId = Guid.NewGuid() };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        valueTrackerList.Should().HaveCount(1); // Verify only 1 entry was created
        valueTrackerList.All(s => s.Description == "One-off cost reduction").Should().BeTrue();
        valueTrackerList.All(s => s.Amount == 500m).Should().BeTrue();
        valueTrackerList.All(s => s.Category == "Billing").Should().BeTrue();
        valueTrackerList.All(s => s.Status == Status.Forecasted).Should().BeTrue();
        valueTrackerList.First().SavingDate.Should().Be(new DateTime(2024, 6, 1)); // Verify the date is correct
        response.SeriesId.Should().NotBeEmpty(); // Verify the series ID is returned
    }
    
    [Fact]
    public async Task CreateSeriesAsync_Create_Monthly_EndDate()
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

        var request = new ValueTrackerCreateSeriesRequest { Description = "Monthly cost reduction", Frequency = Frequency.Monthly, Amount = 1000m, SavingDate = new DateTime(2024, 6, 1), EndCondition = EndCondition.EndDate, EndDate = new DateTime(2024, 12, 31), Status = Status.Forecasted, Category = "Billing", CompanyId = Guid.NewGuid() };

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

    [Fact]
    public async Task CreateSeriesAsync_Create_Monthly_NoEndDate()
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

        var request = new ValueTrackerCreateSeriesRequest
        {
            Description = "Monthly cost reduction",
            Frequency = Frequency.Monthly,
            Amount = 1000m,
            SavingDate = new DateTime(2024, 6, 1),
            EndCondition = EndCondition.NoEndDate, // No end date
            EndDate = null, // Not applicable for NoEndDate
            Status = Status.Forecasted,
            Category = "Billing",
            CompanyId = Guid.NewGuid()
        };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        valueTrackerList.Should().HaveCount(60); // Verify 60 entries were created (5 years * 12 months)
        valueTrackerList.All(s => s.Description == "Monthly cost reduction").Should().BeTrue();
        valueTrackerList.All(s => s.Amount == 1000m).Should().BeTrue();
        valueTrackerList.All(s => s.Category == "Billing").Should().BeTrue();
        valueTrackerList.All(s => s.Status == Status.Forecasted).Should().BeTrue();
        valueTrackerList.Select(s => s.SavingDate).OrderBy(d => d).Should().BeEquivalentTo(
            Enumerable.Range(0, 60).Select(i => new DateTime(2024, 6, 1).AddMonths(i))
        );
        response.SeriesId.Should().NotBeEmpty(); // Verify the series ID is returned
    }

    [Fact]
    public async Task CreateSeriesAsync_Create_Yearly_EndDate()
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

        var request = new ValueTrackerCreateSeriesRequest
        {
            Description = "Yearly cost reduction",
            Frequency = Frequency.Annual,
            Amount = 2000m,
            SavingDate = new DateTime(2024, 6, 1),
            EndCondition = EndCondition.EndDate,
            EndDate = new DateTime(2027, 6, 1), // End date in 3 years
            Status = Status.Forecasted,
            Category = "Billing",
            CompanyId = Guid.NewGuid()
        };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        valueTrackerList.Should().HaveCount(4); // Verify 4 entries were created (2024, 2025, 2026, 2027)
        valueTrackerList.All(s => s.Description == "Yearly cost reduction").Should().BeTrue();
        valueTrackerList.All(s => s.Amount == 2000m).Should().BeTrue();
        valueTrackerList.All(s => s.Category == "Billing").Should().BeTrue();
        valueTrackerList.All(s => s.Status == Status.Forecasted).Should().BeTrue();
        valueTrackerList.Select(s => s.SavingDate).OrderBy(d => d).Should().BeEquivalentTo(new[]
        {
            new DateTime(2024, 6, 1),
            new DateTime(2025, 6, 1),
            new DateTime(2026, 6, 1),
            new DateTime(2027, 6, 1)
        });
        response.SeriesId.Should().NotBeEmpty(); // Verify the series ID is returned
    }

    [Fact]
    public async Task CreateSeriesAsync_Create_Yearly_NoEndDate()
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

        var request = new ValueTrackerCreateSeriesRequest
        {
            Description = "Yearly cost reduction",
            Frequency = Frequency.Annual,
            Amount = 2000m,
            SavingDate = new DateTime(2024, 6, 1),
            EndCondition = EndCondition.NoEndDate, // No end date
            EndDate = null, // Not applicable for NoEndDate
            Status = Status.Forecasted,
            Category = "Billing",
            CompanyId = Guid.NewGuid()
        };

        // Act
        var response = await service.CreateSeriesAsync(request, CancellationToken.None);

        // Assert
        valueTrackerList.Should().HaveCount(5); // Verify 5 entries were created (5 years)
        valueTrackerList.All(s => s.Description == "Yearly cost reduction").Should().BeTrue();
        valueTrackerList.All(s => s.Amount == 2000m).Should().BeTrue();
        valueTrackerList.All(s => s.Category == "Billing").Should().BeTrue();
        valueTrackerList.All(s => s.Status == Status.Forecasted).Should().BeTrue();
        valueTrackerList.Select(s => s.SavingDate).OrderBy(d => d).Should().BeEquivalentTo(new[]
        {
            new DateTime(2024, 6, 1),
            new DateTime(2025, 6, 1),
            new DateTime(2026, 6, 1),
            new DateTime(2027, 6, 1),
            new DateTime(2028, 6, 1)
        });
        response.SeriesId.Should().NotBeEmpty(); // Verify the series ID is returned
    }
}