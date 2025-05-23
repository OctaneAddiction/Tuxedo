using OpenTelemetry.Trace;
using System.Diagnostics;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Api.Admin.ValueTracker.Delete;
using Tuxedo.Api.Admin.ValueTracker.Get;
using Tuxedo.Api.Admin.ValueTracker.Report;
using Tuxedo.Api.Admin.ValueTracker.Update;

namespace Tuxedo.Api.Admin.ValueTracker;

public class ValueTrackerModule : BaseModule
{
	private readonly ILogger<ValueTrackerModule> _logger;

	public ValueTrackerModule(ILogger<ValueTrackerModule> logger) : base("valuetracker")
	{
			_logger = logger;
	}

	public override void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/", async (ValueTrackerCreateRequest req, IValueTrackerCreateService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Create ValueTracker");
			try
			{
				_logger.LogInformation("Creating value tracker: {Request}", req);
				activity?.AddEvent(new ActivityEvent("Creating value tracker"));

				var response = await service.CreateAsync(req, ct);
				return Results.Created($"/admin/valuetracker/{response.Id}", response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating value tracker");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error creating value tracker",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapPost("/series", async (ValueTrackerCreateSeriesRequest req, IValueTrackerCreateService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Create ValueTracker Series");
			try
			{
				_logger.LogInformation("Creating value tracker series: {Request}", req);
				activity?.AddEvent(new ActivityEvent("Creating value tracker series"));

				var response = await service.CreateSeriesAsync(req, ct);
				return Results.Created($"/admin/valuetracker/{response.SeriesId}", response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating value tracker series");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error creating value tracker series",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapPut("/", async (ValueTrackerUpdateRequest req, IValueTrackerUpdateService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Update ValueTracker");
			try
			{
				_logger.LogInformation("Updating value tracker: {Request}", req);
				activity?.AddEvent(new ActivityEvent("Updating value tracker"));

				var response = await service.UpdateAsync(req, ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating value tracker");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error updating value tracker",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/", async (IValueTrackerGetService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Get All ValueTrackers");
			try
			{
				_logger.LogInformation("Getting all value trackers");
				activity?.AddEvent(new ActivityEvent("Getting all value trackers"));

				var response = await service.GetAllAsync(ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting all value trackers");
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error getting all value trackers",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/{id:Guid}", async (Guid id, IValueTrackerGetService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Get ValueTracker");
			try
			{
				_logger.LogInformation("Getting value tracker {Id}", id);
				activity?.AddTag("ValueTracker.Id", id);
				activity?.AddEvent(new ActivityEvent("Getting value tracker"));

				var response = await service.GetByIdAsync(id, ct);
				return response != null ? Results.Ok(response) : Results.NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting value tracker {Id}", id);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error getting value tracker",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/company/{companyId:Guid}", async (Guid companyId, IValueTrackerGetService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Get ValueTrackers By Company");
			try
			{
				_logger.LogInformation("Getting value trackers for company {CompanyId}", companyId);
				activity?.AddTag("Company.Id", companyId);
				activity?.AddEvent(new ActivityEvent("Getting value trackers by company"));
				var response = await service.GetByCompanyIdAsync(companyId, ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting value trackers for company {CompanyId}", companyId);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);
				return Results.Problem("Error getting value trackers by company",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapDelete("/{id:Guid}", async (Guid id, IValueTrackerDeleteService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Delete ValueTracker");
			try
			{
				_logger.LogInformation("Deleting value tracker {Id}", id);
				activity?.AddEvent(new ActivityEvent("Deleting value tracker"));
				activity?.AddTag("ValueTracker.Id", id);

				await service.DeleteAsync(id, ct);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting value tracker {Id}", id);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);

				return Results.Problem("Error deleting value tracker",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});

		app.MapGet("/report/company/{companyId:Guid}", async (Guid companyId, IValueTrackerReportService service, CancellationToken ct) =>
		{
			using var activity = ActivityHelper.Source.StartActivity("Generate ValueTracker Report");
			try
			{
				_logger.LogInformation("Generating value tracker report for company {CompanyId}", companyId);
				activity?.AddTag("Company.Id", companyId);
				activity?.AddEvent(new ActivityEvent("Generating value tracker report"));
				var response = await service.GenerateReportByCompanyIdAsync(companyId, ct);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error generating value tracker report for company {CompanyId}", companyId);
				activity?.RecordException(ex);
				activity?.SetStatus(ActivityStatusCode.Error);
				return Results.Problem("Error generating value tracker report",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		});
	}
}
