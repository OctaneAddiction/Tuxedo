namespace Tuxedo.Api.Admin.ValueTracker.Create;

public interface IValueTrackerCreateService
{
    Task<ValueTrackerCreateResponse> CreateAsync(ValueTrackerCreateRequest request, CancellationToken ct);
}
