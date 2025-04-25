using Microsoft.EntityFrameworkCore;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.ValueTracker.Get;

public class ValueTrackerGetService : IValueTrackerGetService
{
    private readonly ITuxedoDbContext _db;

    public ValueTrackerGetService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<List<ValueTrackerGetResponse>> GetAllAsync(CancellationToken ct)
    {
        return await _db.CompanySaving.Select(s => new ValueTrackerGetResponse
        {
            Id = s.Id,
            Description = s.Description,
            Category = s.Category,
            Amount = s.Amount,
            SavingDate = s.SavingDate,
            CompanyId = s.CompanyId
        }).ToListAsync(ct);
    }

    public async Task<ValueTrackerGetResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var saving = await _db.CompanySaving.FindAsync(new object[] { id }, ct);
        if (saving == null) return null;

        return new ValueTrackerGetResponse
        {
            Id = saving.Id,
            Description = saving.Description,
            Category = saving.Category,
            Amount = saving.Amount,
            SavingDate = saving.SavingDate,
            CompanyId = saving.CompanyId
        };
    }

    public async Task<List<ValueTrackerGetResponse>> GetByCompanyIdAsync(Guid companyId, CancellationToken ct)
    {
		return await _db.CompanySaving
            .Where(s => s.CompanyId == companyId)
			.Select(s => new ValueTrackerGetResponse
		{
			Id = s.Id,
			Description = s.Description,
			Category = s.Category,
			Amount = s.Amount,
			SavingDate = s.SavingDate,
			CompanyId = s.CompanyId
		}).ToListAsync(ct);
	}

}
