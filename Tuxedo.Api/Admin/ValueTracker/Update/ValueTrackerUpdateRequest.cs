namespace Tuxedo.Api.Admin.ValueTracker.Update;

public class ValueTrackerUpdateRequest
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime SavingDate { get; set; }
}
