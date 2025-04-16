using Tuxedo.Api.Responses;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Api.Extensions
{
    public static class CustomerSavingExtensions
    {
        public static CustomerSaving ToEntity(this CreateCustomerSavingRequest request)
        {
            return new CustomerSaving
            {
                ObjectId = request.ObjectId,
                SavingDate = request.SavingDate,
                Description = request.Description,
                Category = request.Category,
                Status = request.Status,
                Amount = request.Amount,
                Frequency = request.Frequency
            };
        }

        public static CustomerSaving ToEntity(this UpdateCustomerSavingRequest request, CustomerSaving existingEntity)
        {
            existingEntity.SavingDate = request.SavingDate;
            existingEntity.Description = request.Description;
            existingEntity.Category = request.Category;
            existingEntity.Status = request.Status;
            existingEntity.Amount = request.Amount;
            existingEntity.Frequency = request.Frequency;
            return existingEntity;
        }

        public static GetCustomerSavingResponse ToResponse(this CustomerSaving entity)
        {
            return new GetCustomerSavingResponse
            {
                ObjectId = entity.ObjectId,
                SavingDate = entity.SavingDate,
                Description = entity.Description,
                Category = entity.Category,
                Status = entity.Status,
                Amount = entity.Amount,
                Frequency = entity.Frequency
            };
        }
    }
}
