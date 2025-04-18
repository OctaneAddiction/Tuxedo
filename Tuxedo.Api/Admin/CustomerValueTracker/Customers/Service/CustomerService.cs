using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Admin.CustomerValueTracker.Customers.Extensions;
using Tuxedo.Api.Admin.CustomerValueTracker.Customers.Request;
using Tuxedo.Api.Admin.CustomerValueTracker.Customers.Response;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.CustomerValueTracker.Customers.Service
{
	public class CustomerService
	{
		private readonly ITuxedoDbContext _db;
		public CustomerService(ITuxedoDbContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<GetCustomerResponse>> GetCustomersAsync()
		{
			var customers = await _db.Customer.ToListAsync();

			if (customers == null || !customers.Any())
			{
				throw new KeyNotFoundException("No customers found.");
			}

			return customers.Select(customer => customer.ToResponse());
		}

		public async Task<GetCustomerResponse> GetCustomerByIdAsync(Guid id)
		{
			var customer = await _db.Customer.FindAsync(id);

			if (customer == null)
			{
				throw new KeyNotFoundException($"Customer with ID {id} not found.");
			}

			return customer.ToResponse();
		}

		public async Task UpdateCustomerAsync(Guid id, UpdateCustomerRequest updateCustomerRequest)
		{
			var customer = await _db.Customer.FindAsync(id);

			if (customer == null)
			{
				throw new KeyNotFoundException($"Customer with ID {id} not found.");
			}

			customer = updateCustomerRequest.ToEntity(customer);

			await _db.SaveChangesAsync();
		}

		public async Task DeleteCustomerAsync(Guid id)
		{
			var customer = await _db.Customer.FindAsync(id);
			if (customer == null)
			{
				throw new KeyNotFoundException($"Customer with ID {id} not found.");
			}

			_db.Customer.Remove(customer);
			await _db.SaveChangesAsync();
		}

		public async Task CreateCustomerAsync(CreateCustomerRequest createCustomerRequest)
		{
			var customer = createCustomerRequest.ToEntity();

			await _db.Customer.AddAsync(customer);
			await _db.SaveChangesAsync();
		}
	}
}
