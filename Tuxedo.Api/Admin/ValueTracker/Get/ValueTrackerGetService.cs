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
		return await _db.ValueTracker.Select(s => new ValueTrackerGetResponse
		{
			Id = s.Id,
			Description = s.Description,
			Category = s.Category,
			Amount = s.Amount,
			SavingDate = s.SavingDate,
			Frequency = s.Frequency,
			Status = s.Status,
			CompanyId = s.CompanyId
		}).ToListAsync(ct);
	}

	public async Task<ValueTrackerGetResponse> GetByIdAsync(Guid id, CancellationToken ct)
	{
		var saving = await _db.ValueTracker.FindAsync(new object[] { id }, ct);
		if (saving == null) return null;

		return new ValueTrackerGetResponse
		{
			Id = saving.Id,
			Description = saving.Description,
			Category = saving.Category,
			Amount = saving.Amount,
			SavingDate = saving.SavingDate,
			Frequency = saving.Frequency,
			Status = saving.Status,
			CompanyId = saving.CompanyId
		};
	}

	public async Task<List<ValueTrackerGetByCompanyResponse>> GetByCompanyIdAsync(Guid companyId, CancellationToken ct)
	{
		var response = new List<ValueTrackerGetByCompanyResponse>();

        var valueTrackers = await _db.ValueTracker
			.Where(s => s.CompanyId == companyId)
			.Select(s => new ValueTrackerGetResponse
			{
				Id = s.Id,
				Description = s.Description,
				Category = s.Category,
				Amount = s.Amount,
				SavingDate = s.SavingDate,
				Frequency = s.Frequency,
				Status = s.Status,
				CompanyId = s.CompanyId
			}).ToListAsync(ct);	

		var totalEstimatedAmountSaved = valueTrackers.Where(x => x.Status == Shared.Enums.Status.Forecasted).Sum(s => s.Amount);
		var totalActualAmountSpent = valueTrackers.Where(x => x.Status == Shared.Enums.Status.Confirmed).Sum(s => s.Amount);
		var totalCount = valueTrackers.Count;
		
		response.Add(new ValueTrackerGetByCompanyResponse
		{
			ValueTrackers = valueTrackers,
			TotalEstimatedAmountSaved = totalEstimatedAmountSaved,
			TotalActualAmountSpent = totalActualAmountSpent,
			TotalCount = totalCount
		});

		return response;
	}
}
