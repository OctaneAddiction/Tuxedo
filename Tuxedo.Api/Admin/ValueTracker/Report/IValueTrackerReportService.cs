namespace Tuxedo.Api.Admin.ValueTracker.Report
{
	public interface IValueTrackerReportService
	{
		Task<ValueTrackerReportResponse> GenerateReportByCompanyIdAsync(Guid companyId, CancellationToken ct);
	}
}