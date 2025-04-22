using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.Company.Update;

public class CompanyUpdateService : ICompanyUpdateService
{
    private readonly ITuxedoDbContext _db;

    public CompanyUpdateService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<CompanyUpdateResponse> UpdateAsync(CompanyUpdateRequest request, CancellationToken ct)
    {
        var company = await _db.Company.FindAsync(new object[] { request.Id }, ct);
        if (company == null) throw new KeyNotFoundException("Company not found");

        company.Name = request.Name;

        await _db.SaveChangesAsync(ct);

        return new CompanyUpdateResponse { Id = company.Id };
    }
}
