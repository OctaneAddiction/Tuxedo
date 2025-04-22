namespace Tuxedo.Api.Admin.Company.Create;

public interface ICompanyCreateService
{
    Task<CompanyCreateResponse> CreateAsync(CompanyCreateRequest request, CancellationToken ct);
}
