using System;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using ImportProcessor.Domain.Managers;
using ImportProcessor.Domain.Managers.Default;
using Moq;
using Xunit;

namespace ImportProcessor.UnitTests.Domain {
	public class AccountManagerTests {

		private const int TenantId = 7;
		private readonly IAccountManager m_sut;
		private readonly Mock<IAccountProvider> m_accountProviderMock;

		public AccountManagerTests() {
			m_accountProviderMock = new Mock<IAccountProvider>( MockBehavior.Loose );
			m_sut = new AccountManager( m_accountProviderMock.Object );
		}

		[Fact]
		public void GetAndUpdateAccount_Exists_Pass() {
			var expectedDto = new AccountDto( TenantId, 8, "A", 9 );

			m_accountProviderMock
				.Setup( x => x.GetAccount( TenantId, "A" ) )
				.Returns( expectedDto );

			var actual = m_sut.GetAndUpdateAccount( TenantId, " A ", 9 );

			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<string>() ), Times.Once);
			Assert.Equal( expectedDto.Type, actual.Type );
			Assert.Equal( expectedDto.Tenant, actual.Tenant );
			Assert.Equal( expectedDto.Id, actual.Id );
			Assert.Equal( expectedDto.Name, actual.Name );
		}

		[Fact]
		public void GetAndUpdateAccount_ExistsShouldUpdated_Pass() {
			const int accountId = 8;
			var expectedDto = new AccountDto( TenantId, accountId, "A", 9 );

			m_accountProviderMock
				.Setup( x => x.GetAccount( TenantId, "A" ) )
				.Returns( expectedDto );
			m_accountProviderMock
				.Setup( x => x.GetAccount( TenantId, accountId ) )
				.Returns( expectedDto );

			m_sut.GetAndUpdateAccount( TenantId, " A ", 10 );

			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<string>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<int>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.UpdateAccount( It.IsAny<AccountDto>() ), Times.Once );
		}

		[Fact]
		public void GetAndUpdateAccount_NotExists_AccountInserted() {
			const int accountId = 8;
			var expectedDto = new AccountDto( TenantId, accountId, "A", 9 );

			m_accountProviderMock
				.Setup( x => x.InsertAccount( It.Is<AccountDto>(
					i => i.Type == 9 && i.Name == "A" && i.Tenant == TenantId
				) ) )
				.Returns( accountId );
			m_accountProviderMock
				.Setup( x => x.GetAccount( TenantId, accountId ) )
				.Returns( expectedDto );

			var actual = m_sut.GetAndUpdateAccount( TenantId, " A ", 9 );

			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<string>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<int>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.InsertAccount( It.IsAny<AccountDto>() ), Times.Once );
			Assert.Equal( expectedDto.Type, actual.Type );
			Assert.Equal( expectedDto.Tenant, actual.Tenant );
			Assert.Equal( expectedDto.Id, actual.Id );
			Assert.Equal( expectedDto.Name, actual.Name );
		}

		[Fact]
		public void GetAndUpdateAccount_NotExistsNotInserted_Fail() {

			Assert.Throws<ApplicationException>( () => { m_sut.GetAndUpdateAccount( TenantId, " A ", 9 ); } );

			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<string>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.GetAccount( It.IsAny<int>(), It.IsAny<int>() ), Times.Once );
			m_accountProviderMock
				.Verify( x => x.InsertAccount( It.IsAny<AccountDto>() ), Times.Once );
		}
	}
}
