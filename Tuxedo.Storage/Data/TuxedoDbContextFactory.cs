using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tuxedo.Storage.Data;

public class TuxedoDbContextFactory : IDesignTimeDbContextFactory<TuxedoDbContext>
{
    public TuxedoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TuxedoDbContext>();
        optionsBuilder.UseSqlite("Data Source=Tuxedo.db");

        return new TuxedoDbContext(optionsBuilder.Options);
    }
}
