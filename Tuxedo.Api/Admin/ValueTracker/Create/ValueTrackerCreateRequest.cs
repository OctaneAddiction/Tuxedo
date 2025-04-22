namespace Tuxedo.Api.Admin.ValueTracker.Create;

public class ValueTrackerCreateRequest
{
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime SavingDate { get; set; }
    public Guid CompanyId { get; set; }
}
