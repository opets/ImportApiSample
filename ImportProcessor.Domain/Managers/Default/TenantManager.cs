using System;
using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using ImportProcessor.Domain.Entities;
using ImportProcessor.Domain.Utils;

namespace ImportProcessor.Domain.Managers.Default {

	internal class TenantManager : ITenantManager {

		private readonly ITenantProvider m_tenantProvider;

		public TenantManager( ITenantProvider tenantProvider ) {
			m_tenantProvider = tenantProvider;
		}

		Tenant ITenantManager.GetAndUpdateTenant( string name ) {
			var normalizedName = (name ?? string.Empty).Trim();

			var dto = m_tenantProvider.GetTenant( normalizedName );
			if (dto == null ) {
				var tenantId = m_tenantProvider.InsertTenant( new TenantDto( 0, normalizedName ) );
				dto = m_tenantProvider.GetTenant( tenantId );

				if(dto == null ) {
					throw new ApplicationException( $"Unable to create Tenant by name '{normalizedName}'" );
				}
			}

			return dto.ToEntity();
		}

		IEnumerable<Tenant> ITenantManager.GetTenants( int pageSize )
			=> m_tenantProvider.GetTenants( pageSize ).Select( x => x?.ToEntity() );

		Tenant ITenantManager.GetTenant( int id )
			=> m_tenantProvider.GetTenant( id )?.ToEntity();

		int ITenantManager.InsertTenant( Tenant tenant )
			=> m_tenantProvider.InsertTenant( tenant.ToDto() );


		void ITenantManager.UpdateTenant( Tenant tenant ) {
			m_tenantProvider.UpdateTenant( tenant.ToDto() );
		}

		void ITenantManager.DeleteTenant( int id ) {
			m_tenantProvider.DeleteTenant( id );
		}

	}
}
