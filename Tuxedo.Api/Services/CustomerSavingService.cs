using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Dtos;
using Tuxedo.Storage.Stores;

namespace Tuxedo.Api.Services;

public class CustomerSavingService
{
    private readonly ITuxedoDbContext _db;
    public CustomerSavingService(ITuxedoDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CustomerSavingDto>> GetCustomerSavingAsync()
    {
        // This method should return a list of customer savings.
                var customerSavings = await _db.CustomerSaving.ToListAsync();
                return customerSavings.Select(cs => new CustomerSavingDto
                {
                    ObjectId = cs.ObjectId,
                    SavingDate = cs.SavingDate,
                    Amount = cs.Amount,
                    Description = cs.Description,
                    Category = cs.Category,
                    Status = cs.Status,
                    Frequency = cs.Frequency
                });
    }
}
