using ImportProcessor.Domain.Entities;

namespace ImportProcessor.Domain.Managers {

	public interface IAccountManager {

		Account GetAndUpdateAccount( int tenantId, string name, short type );

	}

}
