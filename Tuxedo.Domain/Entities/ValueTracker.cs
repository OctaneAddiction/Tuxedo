using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tuxedo.Shared.Enums;

namespace Tuxedo.Domain.Entities;

public class ValueTracker
{
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime SavingDate { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public Status Status { get; set; }
	public decimal Amount { get; set; }
	public Frequency Frequency { get; set; }

	public Guid SeriesId { get; set; } = Guid.NewGuid();

	// Foreign key
	public Guid CompanyId { get; set; }

	// Navigation property
	[ForeignKey("CompanyId")]
	public Company Company { get; set; }
}
