using System.Collections.Generic;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers {

	public interface ITenantProvider {

		IEnumerable<TenantDto> GetTenants( int pageSize = 0 );

		TenantDto GetTenant( int id );

		TenantDto GetTenant( string name );

		int InsertTenant( TenantDto tenant );

		void UpdateTenant( TenantDto tenant );

		void DeleteTenant( int id );

	}

}
