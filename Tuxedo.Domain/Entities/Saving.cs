namespace Tuxedo.Domain.Entities;

public class Saving
{
    public int Id { get; set; }
    public DateTime SavingDate { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public string Frequency { get; set; }
}
