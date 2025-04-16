using Tuxedo.Api.Responses;
using Tuxedo.Domain.Entities;

namespace Tuxedo.Api.Extensions
{
    public static class CustomerExtensions
    {
        public static Customer ToEntity(this CreateCustomerRequest request)
        {
            return new Customer
            {
                Name = request.Name
            };
        }

        public static Customer ToEntity(this UpdateCustomerRequest request, Customer existingEntity)
        {
            existingEntity.Name = request.Name;
            return existingEntity;
        }

        public static GetCustomerResponse ToResponse(this Customer entity)
        {
            return new GetCustomerResponse
            {
                CustomerId = entity.CustomerId,
                Name = entity.Name
            };
        }
    }
}
