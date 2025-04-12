using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;
using Tuxedo.Storage.Data;

namespace Tuxedo.Api.Endpoints;

public static class SavingsEndpoints
{
    public static void MapSavingsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/savings", async (TuxedoDbContext db) =>
        {
            var savings = await db.Savings.ToListAsync();
            return Results.Ok(savings);
        });

        app.MapPost("/api/savings", async (TuxedoDbContext db, Saving saving) =>
        {
            db.Savings.Add(saving);
            await db.SaveChangesAsync();
            return Results.Created($"/api/savings/{saving.Id}", saving);
        });
    }
}