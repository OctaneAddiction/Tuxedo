namespace Tuxedo.Api.Admin.Company.Get;

public interface ICompanyGetService
{
    Task<List<CompanyGetResponse>> GetAllAsync(CancellationToken ct);
    Task<CompanyGetResponse> GetByIdAsync(Guid id, CancellationToken ct);
}
