using System;
using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImportProcessor.IntegrationTests.Data {

	public class AccountProviderTests {

		private const int FakeTenantId = 1;
		private readonly IAccountProvider m_sut;

		public AccountProviderTests() {
			m_sut = DependencyLoader.ServiceLocator.GetService<IAccountProvider>();
		}

		[Fact]
		public void CreateUpdateDelete_Pass() {
			string accountName = $"test account {Guid.NewGuid()}";
			string accountName2 = $"{accountName} 2";

			//Create
			int id = m_sut.InsertAccount( new AccountDto( FakeTenantId, 0, accountName, 3 ) );
			Assert.True( id > 0, "failed on InsertAccount" );

			//Get
			AccountDto account = m_sut.GetAccount( FakeTenantId, id );
			Assert.NotNull( account );
			Assert.Equal( accountName, account.Name );
			Assert.Equal( 3, account.Type );

			try {

				//Update
				m_sut.UpdateAccount( new AccountDto( FakeTenantId, id, accountName2, 4 ) );
				AccountDto updatedAccount = m_sut.GetAccount( FakeTenantId, accountName2 );
				Assert.NotNull( updatedAccount );
				Assert.Equal( id, updatedAccount.Id );
				Assert.Equal( accountName2, updatedAccount.Name );
				Assert.Equal( 4, updatedAccount.Type );

			} finally {

				//Cleanup
				m_sut.DeleteAccount( FakeTenantId, id );
				account = m_sut.GetAccount( FakeTenantId, id );
				Assert.Null( account );
			}
		}

		[Fact]
		public void GetAccounts_Paging_Pass() {
			string accountName = $"test account {Guid.NewGuid()}";

			//Create
			int[] ids = {
				m_sut.InsertAccount( new AccountDto( FakeTenantId, 0, $"{accountName} 1", 11 ) ),
				m_sut.InsertAccount( new AccountDto( FakeTenantId, 0, $"{accountName} 2", 12 ) ),
				m_sut.InsertAccount( new AccountDto( FakeTenantId, 0, $"{accountName} 3", 13 ) )
			};

			try {

				//Get
				IEnumerable<AccountDto> twoAccounts = m_sut.GetAccounts( 2 );
				IEnumerable<AccountDto> allAccounts = m_sut.GetAccounts()
					.Where( x => x.Name.StartsWith( accountName ) );

				Assert.Equal( 2, twoAccounts.Count() );
				Assert.Equal( 3, allAccounts.Count() );

			} finally {

				//Cleanup
				ids.ToList().ForEach( id => m_sut.DeleteAccount( FakeTenantId, id ) );
			}
		}

	}
}
