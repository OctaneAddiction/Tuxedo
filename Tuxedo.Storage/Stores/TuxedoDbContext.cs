using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Stores;

public class TuxedoDbContext : DbContext, ITuxedoDbContext
{
    public TuxedoDbContext(DbContextOptions<TuxedoDbContext> options) : base(options) { }

    public DbSet<CustomerSaving> CustomerSaving { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}