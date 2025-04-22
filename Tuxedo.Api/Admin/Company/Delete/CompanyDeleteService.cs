using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.Company.Delete;

public class CompanyDeleteService : ICompanyDeleteService
{
    private readonly ITuxedoDbContext _db;

    public CompanyDeleteService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var company = await _db.Company.FindAsync(new object[] { id }, ct);
        if (company == null) throw new KeyNotFoundException("Company not found");

        _db.Company.Remove(company);
        await _db.SaveChangesAsync(ct);
    }
}
