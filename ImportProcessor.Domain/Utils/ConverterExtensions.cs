using ImportProcessor.Data.Dto;
using ImportProcessor.Domain.Entities;

namespace ImportProcessor.Domain.Utils {

	internal static class ConverterExtensions {

		internal static Tenant ToEntity( this TenantDto dto )
			=> new Tenant( dto.Id, dto.Name );

		internal static TenantDto ToDto( this Tenant entity )
			=> new TenantDto( entity.Id, entity.Name );

		internal static Account ToEntity( this AccountDto dto )
			=> new Account( dto.Tenant, dto.Id, dto.Name, dto.Type );

		internal static AccountDto ToDto( this Account entity )
			=> new AccountDto( entity.Tenant, entity.Id, entity.Name, entity.Type );

	}

}
