using OpenTelemetry.Trace;
using System.Diagnostics;
using Tuxedo.Api.Admin.Company.Create;
using Tuxedo.Api.Admin.Company.Delete;
using Tuxedo.Api.Admin.Company.Get;
using Tuxedo.Api.Admin.Company.Update;

namespace Tuxedo.Api.Admin.Company;

public class CompanyModule : BaseModule
{
	private readonly ILogger<CompanyModule> _logger;

	public CompanyModule(ILogger<CompanyModule> logger) : base("company")
	{
			_logger = logger;
	}

	public override void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/", async (CompanyCreateRequest req, ICompanyCreateService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Create Company");
			try
			{
				_logger.LogInformation("Creating company: {Request}", req);
				activity?.AddEvent(new ActivityEvent("Creating company"));

				var response = await service.CreateAsync(req, ct);
				return Results.Created($"/admin/company/{response.Id}", response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating company");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error creating company",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapPut("/", async (CompanyUpdateRequest req, ICompanyUpdateService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Update Company");
			try
			{
				_logger.LogInformation("Updating company: {Request}", req);
				activity?.AddEvent(new ActivityEvent("Updating company"));

				var response = await service.UpdateAsync(req, ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating company");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error updating company",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/", async (ICompanyGetService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Get All Companies");
			try
			{
				_logger.LogInformation("Getting all companies");
				activity?.AddEvent(new ActivityEvent("Getting all companies"));

				var response = await service.GetAllAsync(ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting all companies");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error getting all companies",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/{id:Guid}", async (Guid id, ICompanyGetService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Get Company");
			try
			{
				_logger.LogInformation("Getting company {Id}", id);
				activity?.AddTag("Company.Id", id);
				activity?.AddEvent(new ActivityEvent("Getting company"));

				var response = await service.GetByIdAsync(id, ct);
				return response != null ? Results.Ok(response) : Results.NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting company {Id}", id);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error getting company",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapDelete("/{id:Guid}", async (Guid id, ICompanyDeleteService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Delete Company");
			try
			{
				_logger.LogInformation("Deleting company {Id}", id);
				activity?.AddEvent(new ActivityEvent("Deleting company"));
				activity?.AddTag("Company.Id", id);

				await service.DeleteAsync(id, ct);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting company {Id}", id);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error deleting company",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});
	}
}
