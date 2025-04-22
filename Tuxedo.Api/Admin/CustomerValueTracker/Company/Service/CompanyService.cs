using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Extensions;
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Request;
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Response;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.CompanyValueTracker.Company.Service
{
	public class CompanyService
	{
		private readonly ITuxedoDbContext _db;
		public CompanyService(ITuxedoDbContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<GetCompanyResponse>> GetCompaniesAsync()
		{
			var companys = await _db.Company.ToListAsync();

			if (companys == null || !companys.Any())
			{
				throw new KeyNotFoundException("No companys found.");
			}

			return companys.Select(company => company.ToResponse());
		}

		public async Task<GetCompanyResponse> GetCompanyByIdAsync(Guid id)
		{
			var company = await _db.Company.FindAsync(id);

			if (company == null)
			{
				throw new KeyNotFoundException($"Company with ID {id} not found.");
			}

			return company.ToResponse();
		}

		public async Task UpdateCompanyAsync(Guid id, UpdateCompanyRequest updateCompanyRequest)
		{
			var company = await _db.Company.FindAsync(id);

			if (company == null)
			{
				throw new KeyNotFoundException($"Company with ID {id} not found.");
			}

			company = updateCompanyRequest.ToEntity(company);

			await _db.SaveChangesAsync();
		}

		public async Task DeleteCompanyAsync(Guid id)
		{
			var company = await _db.Company.FindAsync(id);
			if (company == null)
			{
				throw new KeyNotFoundException($"Company with ID {id} not found.");
			}

			_db.Company.Remove(company);
			await _db.SaveChangesAsync();
		}

		public async Task CreateCompanyAsync(CreateCompanyRequest createCompanyRequest)
		{
			var company = createCompanyRequest.ToEntity();

			await _db.Company.AddAsync(company);
			await _db.SaveChangesAsync();
		}
	}
}
