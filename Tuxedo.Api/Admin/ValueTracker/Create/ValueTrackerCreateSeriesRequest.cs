using Tuxedo.Shared.Enums;

public class ValueTrackerCreateSeriesRequest
{
    public Frequency Frequency { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime SavingDate { get; set; }
    public Status Status { get; set; }
    public Guid CompanyId { get; set; }
    public EndCondition EndCondition { get; set; } // New field
    public DateTime? EndDate { get; set; } // New field
}