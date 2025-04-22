using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Stores;

public interface ITuxedoDbContext
{
    DbSet<Company> Company { get; set; }
    DbSet<CompanySaving> CompanySaving { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}