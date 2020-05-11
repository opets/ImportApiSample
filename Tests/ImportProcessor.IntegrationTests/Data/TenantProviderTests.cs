using System;
using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace ImportProcessor.IntegrationTests.Data {

	public class TenantProviderTests {

		private readonly ITestOutputHelper m_testOutputHelper;
		private readonly ITenantProvider m_sut;

		public TenantProviderTests( ITestOutputHelper testOutputHelper ) {
			m_testOutputHelper = testOutputHelper;
			m_sut = DependencyLoader.ServiceLocator.GetService<ITenantProvider>();
		}

		[Fact]
		public void CreateUpdateDelete_Pass() {
			string tenantName = $"test tenant {Guid.NewGuid()}";
			string tenantName2 = $"{tenantName} 2";

			//Create
			var id = m_sut.InsertTenant( new TenantDto( 0, tenantName ) );
			Assert.True( id > 0 );

			//Get
			var tenant = m_sut.GetTenant( id );
			Assert.NotNull( tenant );
			Assert.Equal( tenantName, tenant.Name );

			m_testOutputHelper.WriteLine( $"CreateUpdateDelete_Pass.tenantName : {tenantName}: {id}" );

			try {

				//Update
				m_sut.UpdateTenant( new TenantDto( id, tenantName2 ) );
				tenant = m_sut.GetTenant( tenantName2 );
				Assert.NotNull( tenant );
				Assert.Equal( id, tenant.Id );
				Assert.Equal( tenantName2, tenant.Name );

			}
			finally {

				//Delete
				m_sut.DeleteTenant( id );
				tenant = m_sut.GetTenant( id );
				Assert.Null( tenant );
			}
		}

		[Fact]
		public void GetTenants_Paging_Pass() {
			var tenantName = $"test tenant {Guid.NewGuid()}";

			//Create
			var ids = new List<int> {
				m_sut.InsertTenant( new TenantDto( 0, $"{tenantName} 1" ) ),
				m_sut.InsertTenant( new TenantDto( 0, $"{tenantName} 2" ) ),
				m_sut.InsertTenant( new TenantDto( 0, $"{tenantName} 3" ) ),
			};
			ids.ForEach( i => m_testOutputHelper.WriteLine( $"GetTenants_Paging_Pass.tenantName : {tenantName}: {i}" ));

			try {

				//Get
				var twoTenants = m_sut.GetTenants( pageSize: 2 );
				var allTenants = m_sut.GetTenants()
					.Where( x => x.Name.StartsWith( tenantName ) );

				Assert.Equal( 2, twoTenants.Count() );
				Assert.Equal( 3, allTenants.Count() );

			}
			finally {

				//Cleanup
				ids.ForEach( m_sut.DeleteTenant );
			}
		}
	}
}
