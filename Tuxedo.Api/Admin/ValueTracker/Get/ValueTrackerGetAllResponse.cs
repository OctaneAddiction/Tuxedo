using System;

namespace Tuxedo.Api.Admin.ValueTracker.Get;

public class ValueTrackerGetByCompanyResponse
{
    public List<ValueTrackerGetResponse> ValueTrackers { get; set; } = new ();
    public decimal TotalEstimatedAmountSaved { get; set; }
    public decimal TotalActualAmountSpent { get; set; }
    public int TotalCount { get; set; }
}
