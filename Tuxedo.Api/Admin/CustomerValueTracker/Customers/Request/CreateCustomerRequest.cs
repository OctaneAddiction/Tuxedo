
namespace Tuxedo.Api.Admin.CustomerValueTracker.Customers.Request;
public class CreateCustomerRequest
{
	public Guid ObjectId { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
}
