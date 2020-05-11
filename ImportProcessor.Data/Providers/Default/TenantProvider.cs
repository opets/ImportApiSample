using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers.Default {

	internal class TenantProvider : ITenantProvider {

		private readonly IDataRepository m_dataRepository;

		public TenantProvider( IDataRepository dataRepository ) {
			m_dataRepository = dataRepository;
		}

		IEnumerable<TenantDto> ITenantProvider.GetTenants( int pageSize ) {
			string sql = $@"
				SELECT 
					id, 
					name
				FROM tenant
				{( pageSize > 0 ? $"LIMIT {pageSize}" : string.Empty )};
			";

			IEnumerable<TenantDto> tenants = m_dataRepository.Query<TenantDto>(
				sql,
				new { }
			);

			return tenants;
		}

		TenantDto ITenantProvider.GetTenant( int id ) {
			const string sql = @"
				SELECT 
					id, 
					name
				FROM tenant
				WHERE id = @id;
			";
			TenantDto tenant = m_dataRepository.Query<TenantDto>(
				sql,
				new {
					id
				}
			).FirstOrDefault();

			return tenant;
		}

		TenantDto ITenantProvider.GetTenant( string name ) {
			const string sql = @"
				SELECT 
					id, 
					name
				FROM tenant
				WHERE LOWER(name) = LOWER(@name);
			";
			TenantDto tenant = m_dataRepository.Query<TenantDto>(
				sql,
				new {
					name
				}
			).FirstOrDefault();

			return tenant;
		}

		int ITenantProvider.InsertTenant( TenantDto tenant ) {
			const string sql = @"
				INSERT INTO tenant (name) 
				VALUES(@name);

				SELECT LAST_INSERT_ID();
			";
			var id = m_dataRepository.ExecuteScalar<int>(
				sql,
				tenant
			);
			return id;
		}


		void ITenantProvider.UpdateTenant( TenantDto tenant ) {
			const string sql = @"
				UPDATE tenant SET
					name = @name
				WHERE id = @id;
			";
			m_dataRepository.Execute(
				sql,
				tenant
			);
		}

		void ITenantProvider.DeleteTenant( int id ) {
			const string sql = @"
				DELETE FROM tenant
				WHERE id = @id;
			";
			m_dataRepository.Execute(
				sql,
				new {
					id
				}
			);
		}

	}
}
