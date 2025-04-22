using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Admin.Company.Create;

public class CompanyCreateService : ICompanyCreateService
{
    private readonly ITuxedoDbContext _db;

    public CompanyCreateService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<CompanyCreateResponse> CreateAsync(CompanyCreateRequest request, CancellationToken ct)
    {
        var company = new Domain.Entities.Company
        {
            Name = request.Name
        };

        await _db.Company.AddAsync(company, ct);
        await _db.SaveChangesAsync(ct);

        return new CompanyCreateResponse { Id = company.Id };
    }
}
