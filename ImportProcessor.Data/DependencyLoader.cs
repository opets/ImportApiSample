using ImportProcessor.Data.Providers;
using ImportProcessor.Data.Providers.Default;
using Microsoft.Extensions.DependencyInjection;

namespace ImportProcessor.Data {
	public static class DependencyLoader {

		public static void LoadDependencies( IServiceCollection services ) {
			services.AddSingleton<ITenantProvider, TenantProvider>();
			services.AddSingleton<IAccountProvider, AccountProvider>();
			services.AddSingleton<IJournalEntryProvider, JournalEntryProvider>();
			services.AddSingleton<IJournalLineProvider, JournalLineProvider>();
		}
	}
}
