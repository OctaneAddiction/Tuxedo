using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.ValueTracker.Create;

public class ValueTrackerCreateService : IValueTrackerCreateService
{
    private readonly ITuxedoDbContext _db;

    public ValueTrackerCreateService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<ValueTrackerCreateResponse> CreateAsync(ValueTrackerCreateRequest request, CancellationToken ct)
    {
        var saving = new Domain.Entities.ValueTracker
        {
            Frequency = request.Frequency,
            Description = request.Description,
            Category = request.Category,
            Amount = request.Amount,
            SavingDate = request.SavingDate,
            Status = request.Status,
            CompanyId = request.CompanyId
        };

        await _db.ValueTracker.AddAsync(saving, ct);
        await _db.SaveChangesAsync(ct);

        return new ValueTrackerCreateResponse { Id = saving.Id };
    }

    public async Task<ValueTrackerCreateSeriesResponse> CreateSeriesAsync(ValueTrackerCreateSeriesRequest request, CancellationToken ct)
    {
        var savingSeries = new List<Domain.Entities.ValueTracker>();
        var currentDate = request.SavingDate;

        // Generate a unique series ID for the group of entries
        var seriesId = Guid.NewGuid();

        // Determine the end date based on the frequency and end condition
        var endDate = request.EndCondition == Shared.Enums.EndCondition.NoEndDate ? currentDate.AddYears(5) : request.EndDate;

        if (endDate == null)
        {
            throw new ArgumentException("EndDate must be provided if EndCondition is not 'NoEnd'.");
        }

        // Generate records based on the frequency
        while (currentDate <= endDate)
        {
            savingSeries.Add(new Domain.Entities.ValueTracker
            {
                Frequency = request.Frequency,
                Description = request.Description,
                Category = request.Category,
                Amount = request.Amount,
                SavingDate = currentDate,
                Status = request.Status,
                CompanyId = request.CompanyId,
                SeriesId = seriesId // Assign the series ID to each entry
            });

            // Increment the date based on the frequency
            currentDate = request.Frequency switch
            {
                Shared.Enums.Frequency.Monthly => currentDate.AddMonths(1),
                Shared.Enums.Frequency.Quarterly => currentDate.AddMonths(3),
                Shared.Enums.Frequency.Annual => currentDate.AddYears(1),
                _ => throw new ArgumentException("Invalid frequency specified.")
            };
        }

        // Add all generated records to the database
        await _db.ValueTracker.AddRangeAsync(savingSeries, ct);
        await _db.SaveChangesAsync(ct);

        // Return the series ID as a response
        return new ValueTrackerCreateSeriesResponse { SeriesId = seriesId };
    }
}
