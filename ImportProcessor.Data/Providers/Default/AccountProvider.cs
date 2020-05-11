using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers.Default {

	internal class AccountProvider : IAccountProvider {

		private readonly IDataRepository m_dataRepository;

		public AccountProvider( IDataRepository dataRepository ) {
			m_dataRepository = dataRepository;
		}

		IEnumerable<AccountDto> IAccountProvider.GetAccounts( int pageSize ) {
			string sql = $@"
				SELECT 
					tenant,
					id, 
					name,
					`type`
				FROM `account`
				{( pageSize > 0 ? $"LIMIT {pageSize}" : string.Empty )};
			";

			IEnumerable<AccountDto> accounts = m_dataRepository.Query<AccountDto>(
				sql,
				new { }
			);

			return accounts;
		}

		AccountDto IAccountProvider.GetAccount( int tenant, int id ) {
			const string sql = @"
				SELECT 
					tenant,
					id, 
					name,
					`type`
				FROM `account`
				WHERE tenant = @tenant 
					AND id = @id;
			";
			AccountDto account = m_dataRepository.Query<AccountDto>(
				sql,
				new {
					tenant,
					id
				}
			).FirstOrDefault();

			return account;
		}

		AccountDto IAccountProvider.GetAccount( int tenant, string name ) {
			const string sql = @"
				SELECT 
					tenant,
					id, 
					name,
					`type`
				FROM `account`
				WHERE tenant = @tenant 
					AND name = @name;
			";
			AccountDto account = m_dataRepository.Query<AccountDto>(
				sql,
				new {
					tenant,
					name
				}
			).FirstOrDefault();

			return account;
		}

		int IAccountProvider.InsertAccount( AccountDto account ) {
			const string sql = @"
				SET @newId := (SELECT coalesce(MAX(id), 0) + 1 FROM `account` WHERE tenant = @tenant);

				INSERT INTO `account`(tenant, id, name, `type`) 
				VALUES (@tenant, @newId, @name, @type);

				SELECT @newId;
			";
			var res = m_dataRepository.ExecuteScalar<int>(
				sql,
				account
			);

			return res;
		}

		void IAccountProvider.UpdateAccount( AccountDto account ) {
			const string sql = @"
				UPDATE `account` SET
					name = @name,
					`type` = @type
				WHERE tenant = @tenant 
					AND id = @id;
			";
			m_dataRepository.Execute(
				sql,
				account
			);
		}

		void IAccountProvider.DeleteAccount( int tenant, int id ) {
			const string sql = @"
				DELETE FROM `account`
				WHERE tenant = @tenant 
					AND id = @id;
			";
			m_dataRepository.Execute(
				sql,
				new {
					tenant,
					id
				}
			);
		}

	}
}
