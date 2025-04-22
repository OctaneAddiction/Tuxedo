
namespace Tuxedo.Api.Admin.CustomerValueTracker.Company.Request;
public class CreateCompanyRequest
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
}
