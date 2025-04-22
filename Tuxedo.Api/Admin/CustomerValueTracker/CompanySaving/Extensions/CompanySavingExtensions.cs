
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Request;
using Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Response;

namespace Tuxedo.Api.Admin.CustomerValueTracker.CompanySaving.Extensions;

	public static class CompanySavingExtensions
	{
		public static Domain.Entities.CompanySaving ToEntity(this CreateCompanySavingRequest request)
		{
			return new Domain.Entities.CompanySaving
			{
				Id = request.Id,
				SavingDate = request.SavingDate,
				Description = request.Description,
				Category = request.Category,
				Status = request.Status,
				Amount = request.Amount,
				Frequency = request.Frequency,
				CompanyId = request.CompanyId
			};
		}

		public static Domain.Entities.CompanySaving ToEntity(this UpdateCompanySavingRequest request, Domain.Entities.CompanySaving existingEntity)
		{
			existingEntity.SavingDate = request.SavingDate;
			existingEntity.Description = request.Description;
			existingEntity.Category = request.Category;
			existingEntity.Status = request.Status;
			existingEntity.Amount = request.Amount;
			existingEntity.Frequency = request.Frequency;
			existingEntity.CompanyId = request.CompanyId;
			return existingEntity;
		}

		public static GetCompanySavingResponse ToResponse(this Domain.Entities.CompanySaving entity)
		{
			return new GetCompanySavingResponse
			{
				Id = entity.Id,
				SavingDate = entity.SavingDate,
				Description = entity.Description,
				Category = entity.Category,
				Status = entity.Status,
				Amount = entity.Amount,
				Frequency = entity.Frequency,
				CompanyId = entity.CompanyId
			};
		}
	}

