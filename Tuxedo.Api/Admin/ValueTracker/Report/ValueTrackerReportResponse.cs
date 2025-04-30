namespace Tuxedo.Api.Admin.ValueTracker.Report
{
	public class ValueTrackerReportResponse
	{
		public Guid CompanyId { get; set; }
		public decimal TotalEstimatedAmountSaved { get; set; }
		public decimal TotalActualAmountSpent { get; set; }
		public int TotalCount { get; set; }
		public List<ValueTrackerReportItem> ValueTrackers { get; set; } = new();
	}
}
