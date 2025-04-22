using Tuxedo.Domain.Entities;
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
        var saving = new CompanySaving
        {
            Description = request.Description,
            Category = request.Category,
            Amount = request.Amount,
            SavingDate = request.SavingDate,
            CompanyId = request.CompanyId
        };

        await _db.CompanySaving.AddAsync(saving, ct);
        await _db.SaveChangesAsync(ct);

        return new ValueTrackerCreateResponse { Id = saving.Id };
    }
}
