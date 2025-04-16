public class CreateCustomerRequest
{
	public Guid ObjectId { get; set; } = Guid.NewGuid();
	public string Name { get; set; }
}
