using System;
using System.IO;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Common.Default;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportProcessor.IntegrationTests {
	public static class DependencyLoader {

		public static ServiceProvider ServiceLocator = BuildDependencies();

		public static IConfiguration Configuration { get; set; }

		public static ServiceProvider BuildDependencies(Action<IServiceCollection> preBuildAction = null) {
			var services = new ServiceCollection();

			var builder = new ConfigurationBuilder()
				.SetBasePath( Directory.GetCurrentDirectory() )
				.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true );
			Configuration = builder.Build();
			string connectionString = Configuration["Data:DefaultConnection:ConnectionString"];

			services.AddSingleton<IDataRepository>( a => new DataRepository( connectionString ) );

			ImportProcessor.Domain.DependencyLoader.LoadDependencies( services );

			preBuildAction?.Invoke( services );

			return services.BuildServiceProvider();
		}

	}
}
