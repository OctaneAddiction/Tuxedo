using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Extensions;
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Request;
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Response;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.CompanyValueTracker.CompanySaving.Service;

public class CompanySavingService
{
	private readonly ITuxedoDbContext _db;
	public CompanySavingService(ITuxedoDbContext db)
	{
		_db = db;
	}

	public async Task<IEnumerable<GetCompanySavingResponse>> GetCompanySavingAsync()
	{
		var companySavings = await _db.CompanySaving.ToListAsync();

		if (companySavings == null || !companySavings.Any())
		{
			throw new KeyNotFoundException("No company savings found.");
		}

		return companySavings.Select(saving => saving.ToResponse());
	}

	public async Task<GetCompanySavingResponse> GetCompanySavingByIdAsync(Guid id)
	{
		var companySaving = await _db.CompanySaving.FindAsync(id);

		if (companySaving == null)
		{
			throw new KeyNotFoundException($"Company saving with ID {id} not found.");
		}

		return companySaving.ToResponse();
	}

	public async Task UpdateCompanySavingAsync(Guid id, UpdateCompanySavingRequest updateCompanySavingRequest)
	{
		var companySaving = await _db.CompanySaving.FindAsync(id);

		if (companySaving == null)
		{
			throw new KeyNotFoundException($"Company saving with ID {id} not found.");
		}

		companySaving = updateCompanySavingRequest.ToEntity(companySaving);

		await _db.SaveChangesAsync();
	}

	public async Task DeleteCompanySavingAsync(Guid id)
	{
		var companySaving = await _db.CompanySaving.FindAsync(id);
		if (companySaving == null)
		{
			throw new KeyNotFoundException($"Company saving with ID {id} not found.");
		}

		_db.CompanySaving.Remove(companySaving);
		await _db.SaveChangesAsync();
	}

	public async Task CreateCompanySavingAsync(CreateCompanySavingRequest createCompanySavingRequest)
	{
		var companySaving = createCompanySavingRequest.ToEntity();

		await _db.CompanySaving.AddAsync(companySaving);
		await _db.SaveChangesAsync();
	}
}
