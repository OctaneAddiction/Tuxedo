namespace Tuxedo.Api.Admin.ValueTracker.Delete;

public interface IValueTrackerDeleteService
{
    Task DeleteAsync(Guid id, CancellationToken ct);
}
