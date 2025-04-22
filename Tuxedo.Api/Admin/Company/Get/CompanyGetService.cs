using Microsoft.EntityFrameworkCore;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.Company.Get;

public class CompanyGetService : ICompanyGetService
{
    private readonly ITuxedoDbContext _db;

    public CompanyGetService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<List<CompanyGetResponse>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Company.Select(c => new CompanyGetResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync(ct);
    }

    public async Task<CompanyGetResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var company = await _db.Company.FindAsync(new object[] { id }, ct);
        if (company == null) return null;

        return new CompanyGetResponse
        {
            Id = company.Id,
            Name = company.Name
        };
    }
}
