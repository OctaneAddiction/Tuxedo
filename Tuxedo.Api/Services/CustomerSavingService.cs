using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Extensions;
using Tuxedo.Api.Responses;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Services;

public class CustomerSavingService
{
	private readonly ITuxedoDbContext _db;
	public CustomerSavingService(ITuxedoDbContext db)
	{
		_db = db;
	}

	public async Task<IEnumerable<GetCustomerSavingResponse>> GetCustomerSavingAsync()
	{
		var customerSavings = await _db.CustomerSaving.ToListAsync();

		if (customerSavings == null || !customerSavings.Any())
		{
			throw new KeyNotFoundException("No customer savings found.");
		}

		return customerSavings.Select(saving => saving.ToResponse());
	}

	public async Task<GetCustomerSavingResponse> GetCustomerSavingByIdAsync(Guid id)
	{
		var customerSaving = await _db.CustomerSaving.FindAsync(id);

		if (customerSaving == null)
		{
			throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
		}

		return customerSaving.ToResponse();
	}

	public async Task UpdateCustomerSavingAsync(Guid id, UpdateCustomerSavingRequest updateCustomerSavingRequest)
	{
		var customerSaving = await _db.CustomerSaving.FindAsync(id);

		if (customerSaving == null)
		{
			throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
		}

		customerSaving = updateCustomerSavingRequest.ToEntity(customerSaving);

		await _db.SaveChangesAsync();
	}

	public async Task DeleteCustomerSavingAsync(Guid id)
	{
		var customerSaving = await _db.CustomerSaving.FindAsync(id);
		if (customerSaving == null)
		{
			throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
		}

		_db.CustomerSaving.Remove(customerSaving);
		await _db.SaveChangesAsync();
	}

	public async Task CreateCustomerSavingAsync(CreateCustomerSavingRequest createCustomerSavingRequest)
	{
		var customerSaving = createCustomerSavingRequest.ToEntity();

		await _db.CustomerSaving.AddAsync(customerSaving);
		await _db.SaveChangesAsync();
	}
}
