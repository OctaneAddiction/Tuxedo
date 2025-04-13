using Microsoft.EntityFrameworkCore;
using Tuxedo.Api.Dtos;
using Tuxedo.Domain.Entities;
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

    public async Task<CustomerSavingDto> GetCustomerSavingByIdAsync(Guid id)
    {
        // This method should return a single customer saving by ID.
        var customerSaving = await _db.CustomerSaving.FindAsync(id);
        if (customerSaving == null)
        {
            throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
        }

        return new CustomerSavingDto
        {
            ObjectId = customerSaving.ObjectId,
            SavingDate = customerSaving.SavingDate,
            Amount = customerSaving.Amount,
            Description = customerSaving.Description,
            Category = customerSaving.Category,
            Status = customerSaving.Status,
            Frequency = customerSaving.Frequency
        };
    }
    
    public async Task UpdateCustomerSavingAsync(Guid id, CustomerSavingDto customerSavingDto)
    {
        // This method should update an existing customer saving.
        var customerSaving = await _db.CustomerSaving.FindAsync(id);
        if (customerSaving == null)
        {
            throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
        }

        customerSaving.SavingDate = customerSavingDto.SavingDate;
        customerSaving.Amount = customerSavingDto.Amount;
        customerSaving.Description = customerSavingDto.Description;
        customerSaving.Category = customerSavingDto.Category;
        customerSaving.Status = customerSavingDto.Status;
        customerSaving.Frequency = customerSavingDto.Frequency;

        await _db.SaveChangesAsync();
    }
    public async Task DeleteCustomerSavingAsync(Guid id)
    {
        // This method should delete a customer saving.
        var customerSaving = await _db.CustomerSaving.FindAsync(id);
        if (customerSaving == null)
        {
            throw new KeyNotFoundException($"Customer saving with ID {id} not found.");
        }

        _db.CustomerSaving.Remove(customerSaving);
        await _db.SaveChangesAsync();
    }

    public async Task CreateCustomerSavingAsync(CustomerSavingDto customerSavingDto)
    {
        // This method should add a new customer saving.
        var customerSaving = new CustomerSaving
        {
            SavingDate = customerSavingDto.SavingDate,
            Amount = customerSavingDto.Amount,
            Description = customerSavingDto.Description,
            Category = customerSavingDto.Category,
            Status = customerSavingDto.Status,
            Frequency = customerSavingDto.Frequency
        };

        await _db.CustomerSaving.AddAsync(customerSaving);
        await _db.SaveChangesAsync();
    }
}
