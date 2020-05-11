using System;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using ImportProcessor.Domain.Entities;
using ImportProcessor.Domain.Utils;

namespace ImportProcessor.Domain.Managers.Default {

	internal class AccountManager : IAccountManager {

		private readonly IAccountProvider m_accountProvider;

		public AccountManager( IAccountProvider accountProvider ) {
			m_accountProvider = accountProvider;
		}

		Account IAccountManager.GetAndUpdateAccount( int tenantId, string name, short type ) {
			var normalizedName = (name ?? string.Empty).Trim();

			var dto = m_accountProvider.GetAccount( tenantId, normalizedName );
			if (dto == null ) {
				var accountId = m_accountProvider.InsertAccount( new AccountDto( tenantId, 0, normalizedName, type ) );
				dto = m_accountProvider.GetAccount( tenantId, accountId );
			}

			if( dto != null && dto.Type != type ) {
				m_accountProvider.UpdateAccount( new AccountDto( tenantId, dto.Id, dto.Name, type ) );
				dto = m_accountProvider.GetAccount( tenantId, dto.Id );
			}

			if( dto == null ) {
				throw new ApplicationException( $"Unable to create Account by name '{normalizedName}'" );
			}

			return dto.ToEntity();
		}

	}
}
