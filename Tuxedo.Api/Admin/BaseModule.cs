using Carter;

namespace Tuxedo.Api.Admin;

public abstract class BaseModule : CarterModule
{
	protected BaseModule(string path) : base($"api/{path}")
	{
		//RequireAuthorization();
	}
}