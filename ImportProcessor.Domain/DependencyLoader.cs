using ImportProcessor.Domain.Managers;
using ImportProcessor.Domain.Managers.Default;
using ImportProcessor.Domain.Parsers;
using ImportProcessor.Domain.Parsers.Default;
using ImportProcessor.Domain.Processors;
using ImportProcessor.Domain.Processors.Default;
using Microsoft.Extensions.DependencyInjection;

namespace ImportProcessor.Domain {

	public static class DependencyLoader {

		public static void LoadDependencies( IServiceCollection services ) {

			//base
			Data.DependencyLoader.LoadDependencies( services );

			//managers
			services.AddSingleton<ITenantManager, TenantManager>();
			services.AddSingleton<IAccountManager, AccountManager>();

			//parsers
			services.AddSingleton<ILedgerParser, LedgerParser>();

			//processors
			services.AddSingleton<ILedgerProcessor, LedgerProcessor>();
			services.AddSingleton<IMainProcessor, MainProcessor>();

		}
	}
}
