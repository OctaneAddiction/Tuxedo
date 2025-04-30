using Microsoft.EntityFrameworkCore;
using Tuxedo.Shared.Enums;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.ValueTracker.Report;

public class ValueTrackerReportService : IValueTrackerReportService
{
    private readonly ITuxedoDbContext _db;

    public ValueTrackerReportService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<ValueTrackerReportResponse> GenerateReportByCompanyIdAsync(Guid companyId, CancellationToken ct)
    {
        var valueTrackers = await _db.ValueTracker
            .Where(vt => vt.CompanyId == companyId)
            .ToListAsync(ct);

        if (!valueTrackers.Any())
        {
            throw new KeyNotFoundException("No value trackers found for the specified company.");
        }

        var totalEstimatedAmountSaved = valueTrackers
            .Where(vt => vt.Status == Status.Forecasted)
            .Sum(vt => vt.Amount);

        var totalActualAmountSpent = valueTrackers
            .Where(vt => vt.Status == Status.Confirmed)
            .Sum(vt => vt.Amount);

        var report = new ValueTrackerReportResponse
        {
            CompanyId = companyId,
            TotalEstimatedAmountSaved = totalEstimatedAmountSaved,
            TotalActualAmountSpent = totalActualAmountSpent,
            TotalCount = valueTrackers.Count,
            ValueTrackers = valueTrackers.Select(vt => new ValueTrackerReportItem
            {
                Id = vt.Id,
                Description = vt.Description,
                Category = vt.Category,
                Amount = vt.Amount,
                SavingDate = vt.SavingDate,
                Status = vt.Status,
                Frequency = vt.Frequency
            }).ToList()
        };

        return report;
    }
}




