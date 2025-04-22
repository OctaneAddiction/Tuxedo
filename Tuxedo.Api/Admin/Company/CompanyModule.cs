using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Tuxedo.Api.Admin.Company.Create;
using Tuxedo.Api.Admin.Company.Delete;
using Tuxedo.Api.Admin.Company.Get;
using Tuxedo.Api.Admin.Company.Update;

namespace Tuxedo.Api.Admin.Company;

public class CompanyModule
{
    public static void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CompanyCreateRequest req, ICompanyCreateService service, CancellationToken ct) =>
        {
            var response = await service.CreateAsync(req, ct);
            return Results.Created($"/admin/company/{response.Id}", response);
        });

        app.MapPut("/", async (CompanyUpdateRequest req, ICompanyUpdateService service, CancellationToken ct) =>
        {
            var response = await service.UpdateAsync(req, ct);
            return Results.Ok(response);
        });

        app.MapGet("/", async (ICompanyGetService service, CancellationToken ct) =>
        {
            var response = await service.GetAllAsync(ct);
            return Results.Ok(response);
        });

        app.MapGet("/{id:Guid}", async (Guid id, ICompanyGetService service, CancellationToken ct) =>
        {
            var response = await service.GetByIdAsync(id, ct);
            return response != null ? Results.Ok(response) : Results.NotFound();
        });

        app.MapDelete("/{id:Guid}", async (Guid id, ICompanyDeleteService service, CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });
    }
}
