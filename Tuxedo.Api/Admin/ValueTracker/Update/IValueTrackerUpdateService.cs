namespace Tuxedo.Api.Admin.ValueTracker.Update;

public interface IValueTrackerUpdateService
{
    Task<ValueTrackerUpdateResponse> UpdateAsync(ValueTrackerUpdateRequest request, CancellationToken ct);
}
