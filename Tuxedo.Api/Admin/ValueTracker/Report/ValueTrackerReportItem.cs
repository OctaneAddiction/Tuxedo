using Tuxedo.Shared.Enums;

namespace Tuxedo.Api.Admin.ValueTracker.Report
{
	public class ValueTrackerReportItem
	{
		public Guid Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public decimal Amount { get; set; }
		public DateTime SavingDate { get; set; }
		public Status Status { get; set; }
		public Frequency Frequency { get; set; }
	}
}
