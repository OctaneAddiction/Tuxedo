using Carter;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tuxedo.Api.Admin.Company.Create;
using Tuxedo.Api.Admin.Company.Delete;
using Tuxedo.Api.Admin.Company.Get;
using Tuxedo.Api.Admin.Company.Update;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Api.Admin.ValueTracker.Delete;
using Tuxedo.Api.Admin.ValueTracker.Get;
using Tuxedo.Api.Admin.ValueTracker.Report;
using Tuxedo.Api.Admin.ValueTracker.Update;
using Tuxedo.Storage;
using Tuxedo.Storage.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

// Configure EF Core with SQLite
builder.Services.AddDbContext<TuxedoDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITuxedoDbContext, TuxedoDbContext>();

builder.Services.AddTransient<ICompanyCreateService, CompanyCreateService>();
builder.Services.AddTransient<ICompanyUpdateService, CompanyUpdateService>();
builder.Services.AddTransient<ICompanyDeleteService, CompanyDeleteService>();
builder.Services.AddTransient<ICompanyGetService, CompanyGetService>();

builder.Services.AddTransient<IValueTrackerCreateService, ValueTrackerCreateService>();
builder.Services.AddTransient<IValueTrackerUpdateService, ValueTrackerUpdateService>();
builder.Services.AddTransient<IValueTrackerDeleteService, ValueTrackerDeleteService>();
builder.Services.AddTransient<IValueTrackerGetService, ValueTrackerGetService>();
builder.Services.AddTransient<IValueTrackerReportService, ValueTrackerReportService>();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "Tuxedo API",
		Version = "v1",
		Description = "API documentation for the Tuxedo application.",
		Contact = new Microsoft.OpenApi.Models.OpenApiContact
		{
			Name = "Tuxedo Team",
			Email = "support@tuxedo.com",
			Url = new Uri("https://tuxedo.com")
		}
	});

	// Add XML comments if available
	var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	if (File.Exists(xmlPath))
	{
		options.IncludeXmlComments(xmlPath);
	}
});

// Register Carter
builder.Services.AddCarter();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ITuxedoDbContext>();
	DatabaseSeeder.Seed(dbContext);
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tuxedo API v1");
		options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
	});
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

// Use Carter
app.MapCarter();

app.Run();

public static class ActivityHelper
{
	public static ActivitySource Source = new ActivitySource("Tuxedo");
}