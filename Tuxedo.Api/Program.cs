using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Routes;
using Tuxedo.Api.Services;
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
builder.Services.AddScoped<CustomerSavingService, CustomerSavingService>();
builder.Services.AddScoped<CustomerService, CustomerService>();

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

app.RegisterCustomerRoutes();

app.RegisterCustomerSavingRoutes();


app.Run();