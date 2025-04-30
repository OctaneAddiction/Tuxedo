namespace Tuxedo.Api.Admin.ValueTracker.Get;

public class ValueTrackerGetByCompanyResponse
{
    public List<ValueTrackerGetResponse> ValueTrackers { get; set; } = new ();

    public int TotalCount { get; set; }
}
