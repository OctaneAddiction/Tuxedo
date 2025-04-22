using System;
using Tuxedo.Shared.Enums;

namespace Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Request;

public class UpdateCompanySavingRequest
{
	public DateTime SavingDate { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public Status Status { get; set; }
	public decimal Amount { get; set; }
	public Frequency Frequency { get; set; }
	public Guid CompanyId { get; set; }
}
