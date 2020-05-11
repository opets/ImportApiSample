using System.Collections.Generic;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers {

	public interface IAccountProvider {

		IEnumerable<AccountDto> GetAccounts( int pageSize = 0 );

		AccountDto GetAccount( int tenant, int id );

		AccountDto GetAccount( int tenant, string name );

		int InsertAccount( AccountDto account );

		void UpdateAccount( AccountDto account );

		void DeleteAccount( int tenant, int id );

	}

}
