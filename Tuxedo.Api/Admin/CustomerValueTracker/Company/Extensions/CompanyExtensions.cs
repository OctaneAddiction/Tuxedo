
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Request;
using Tuxedo.Api.Admin.CustomerValueTracker.Company.Response;

namespace Tuxedo.Api.Admin.CustomerValueTracker.Company.Extensions;

public static class CompanyExtensions
{
	public static Domain.Entities.Company ToEntity(this CreateCompanyRequest request)
	{
		return new Domain.Entities.Company
		{
			Id = request.Id,
			Name = request.Name
		};
	}

	public static Domain.Entities.Company ToEntity(this UpdateCompanyRequest request, Domain.Entities.Company existingEntity)
	{
		existingEntity.Name = request.Name;
		return existingEntity;
	}

	public static GetCompanyResponse ToResponse(this Domain.Entities.Company entity)
	{
		return new GetCompanyResponse
		{
			Id = entity.Id,
			Name = entity.Name
		};
	}
}
