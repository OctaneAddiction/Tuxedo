using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Api.Admin.ValueTracker.Delete;
using Tuxedo.Api.Admin.ValueTracker.Get;
using Tuxedo.Api.Admin.ValueTracker.Update;

namespace Tuxedo.Api.Admin.ValueTracker;

public class ValueTrackerModule
{
    public static void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (ValueTrackerCreateRequest req, IValueTrackerCreateService service, CancellationToken ct) =>
        {
            var response = await service.CreateAsync(req, ct);
            return Results.Created($"/admin/valuetracker/{response.Id}", response);
        });

        app.MapPut("/", async (ValueTrackerUpdateRequest req, IValueTrackerUpdateService service, CancellationToken ct) =>
        {
            var response = await service.UpdateAsync(req, ct);
            return Results.Ok(response);
        });

        app.MapGet("/", async (IValueTrackerGetService service, CancellationToken ct) =>
        {
            var response = await service.GetAllAsync(ct);
            return Results.Ok(response);
        });

        app.MapGet("/{id:Guid}", async (Guid id, IValueTrackerGetService service, CancellationToken ct) =>
        {
            var response = await service.GetByIdAsync(id, ct);
            return response != null ? Results.Ok(response) : Results.NotFound();
        });

        app.MapDelete("/{id:Guid}", async (Guid id, IValueTrackerDeleteService service, CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });
    }
}
