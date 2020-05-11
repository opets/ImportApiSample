using System.Collections.Generic;
using ImportProcessor.Domain.Entities;

namespace ImportProcessor.Domain.Managers {

	public interface ITenantManager {

		Tenant GetAndUpdateTenant( string name );

		IEnumerable<Tenant> GetTenants( int pageSize = 0 );

		Tenant GetTenant( int id );

		int InsertTenant( Tenant tenant );

		void UpdateTenant( Tenant tenant );

		void DeleteTenant( int id );

	}

}
