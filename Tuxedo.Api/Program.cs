using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tuxedo.Api.Admin.Company.Create;
using Tuxedo.Api.Admin.Company.Delete;
using Tuxedo.Api.Admin.Company.Get;
using Tuxedo.Api.Admin.Company.Update;
using Tuxedo.Api.Admin.ValueTracker.Create;
using Tuxedo.Api.Admin.ValueTracker.Delete;
using Tuxedo.Api.Admin.ValueTracker.Get;
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

// Use the SavingsEndpoints extension method



app.Run();

public static class ActivityHelper
{
	public static ActivitySource Source = new ActivitySource("Tuxedo");
}