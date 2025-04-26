namespace Tuxedo.Api.Admin.ValueTracker.Get;

public interface IValueTrackerGetService
{
    Task<List<ValueTrackerGetResponse>> GetAllAsync(CancellationToken ct);
    Task<ValueTrackerGetResponse> GetByIdAsync(Guid id, CancellationToken ct);
	Task<List<ValueTrackerGetByCompanyResponse>> GetByCompanyIdAsync(Guid companyId, CancellationToken ct);
}
