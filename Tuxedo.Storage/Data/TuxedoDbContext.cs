using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Data;

public class TuxedoDbContext : DbContext
{
    public TuxedoDbContext(DbContextOptions<TuxedoDbContext> options) : base(options) { }

    public DbSet<Saving> Savings { get; set; }
}