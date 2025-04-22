namespace Tuxedo.Api.Admin.Company.Delete;

public interface ICompanyDeleteService
{
    Task DeleteAsync(Guid id, CancellationToken ct);
}
