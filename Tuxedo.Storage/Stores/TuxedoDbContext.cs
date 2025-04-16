using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Stores;

public class TuxedoDbContext : DbContext, ITuxedoDbContext
{
	public TuxedoDbContext(DbContextOptions<TuxedoDbContext> options) : base(options) { }

	public DbSet<Customer> Customer { get; set; }
	public DbSet<CustomerSaving> CustomerSaving { get; set; }

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure one-to-many relationship
		modelBuilder.Entity<Customer>()
			.HasMany(c => c.CustomerSavings)
			.WithOne(s => s.Customer)
			.HasForeignKey(s => s.CustomerId);

		// Configure Customer entity
		modelBuilder.Entity<Customer>(entity =>
		{
			entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
		});

		// Configure CustomerSaving entity
		modelBuilder.Entity<CustomerSaving>(entity =>
		{
			entity.Property(e => e.Description).IsRequired().HasMaxLength(250);
			entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
		});
	}
}
