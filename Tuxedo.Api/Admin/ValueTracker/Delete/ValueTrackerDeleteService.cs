using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.ValueTracker.Delete;

public class ValueTrackerDeleteService : IValueTrackerDeleteService
{
    private readonly ITuxedoDbContext _db;

    public ValueTrackerDeleteService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var saving = await _db.CompanySaving.FindAsync(new object[] { id }, ct);
        if (saving == null) throw new KeyNotFoundException("Saving not found");

        _db.CompanySaving.Remove(saving);
        await _db.SaveChangesAsync(ct);
    }
}
