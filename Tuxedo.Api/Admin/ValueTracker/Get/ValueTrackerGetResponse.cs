namespace Tuxedo.Api.Admin.ValueTracker.Get;

public class ValueTrackerGetResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime SavingDate { get; set; }
    public Guid CompanyId { get; set; }
}
