using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.ValueTracker.Update;

public class ValueTrackerUpdateService : IValueTrackerUpdateService
{
    private readonly ITuxedoDbContext _db;

    public ValueTrackerUpdateService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<ValueTrackerUpdateResponse> UpdateAsync(ValueTrackerUpdateRequest request, CancellationToken ct)
    {
        var saving = await _db.ValueTracker.FindAsync(new object[] { request.Id }, ct);
        if (saving == null) throw new KeyNotFoundException("Saving not found");

        saving.Description = request.Description;
        saving.Category = request.Category;
        saving.Amount = request.Amount;
        saving.SavingDate = request.SavingDate;
        saving.Status = request.Status;
        saving.Frequency = request.Frequency;

        await _db.SaveChangesAsync(ct);

        return new ValueTrackerUpdateResponse { Id = saving.Id };
    }
}
