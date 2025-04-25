using Microsoft.EntityFrameworkCore;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Storage.Stores;

public class TuxedoDbContext : DbContext, ITuxedoDbContext
{
	public TuxedoDbContext(DbContextOptions<TuxedoDbContext> options) : base(options) { }

	public DbSet<Company> Company { get; set; }
	public DbSet<ValueTracker> ValueTracker { get; set; }

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure one-to-many relationship
		modelBuilder.Entity<Company>()
			.HasMany(c => c.ValueTrackers)
			.WithOne(s => s.Company)
			.HasForeignKey(s => s.CompanyId);

		// Configure Customer entity
		modelBuilder.Entity<Company>(entity =>
		{
			entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
		});

		// Configure CustomerSaving entity
		modelBuilder.Entity<ValueTracker>(entity =>
		{
			entity.Property(e => e.Description).IsRequired().HasMaxLength(250);
			entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
		});
	}
}
