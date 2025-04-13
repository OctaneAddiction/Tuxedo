using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Routes;
using Tuxedo.Storage.Data;
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


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TuxedoDbContext>();
    DatabaseSeeder.Seed(db); // Call the seeding logic
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

// Use the SavingsEndpoints extension method
app.RegisterCustomerSavingRoutes();

app.Run();