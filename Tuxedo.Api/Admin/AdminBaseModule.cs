using Carter;

namespace Tuxedo.Api.Admin;

public abstract class AdminBaseModule : CarterModule
{
	protected AdminBaseModule(string path) : base($"admin/{path}")
	{
		RequireAuthorization();
	}
}