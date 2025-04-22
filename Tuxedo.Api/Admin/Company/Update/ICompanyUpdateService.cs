namespace Tuxedo.Api.Admin.Company.Update;

public interface ICompanyUpdateService
{
    Task<CompanyUpdateResponse> UpdateAsync(CompanyUpdateRequest request, CancellationToken ct);
}
