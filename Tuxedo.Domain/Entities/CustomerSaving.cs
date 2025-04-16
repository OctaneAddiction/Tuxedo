using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tuxedo.Shared.Enums;

namespace Tuxedo.Domain.Entities;

public class CustomerSaving
{
	[Key]
	public Guid ObjectId { get; set; } = Guid.NewGuid();
	public DateTime SavingDate { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public Status Status { get; set; }
	public decimal Amount { get; set; }
	public Frequency Frequency { get; set; }

	// Foreign key
	public Guid CustomerId { get; set; }

	// Navigation property
	[ForeignKey("CustomerId")]
	public Customer Customer { get; set; }
}
