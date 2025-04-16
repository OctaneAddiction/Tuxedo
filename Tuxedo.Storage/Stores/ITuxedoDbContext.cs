using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Stores;

public interface ITuxedoDbContext
{
    DbSet<Customer> Customer { get; set; }
    DbSet<CustomerSaving> CustomerSaving { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}