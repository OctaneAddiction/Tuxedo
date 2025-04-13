using System;
using Tuxedo.Shared.Enums;

namespace Tuxedo.Api.Responses;

public class GetCustomerSavingResponse
{
    public Guid ObjectId { get; set; } = Guid.NewGuid();
    public DateTime SavingDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Status Status { get; set; }
    public decimal Amount { get; set; }
    public Frequency Frequency { get; set; }
}
