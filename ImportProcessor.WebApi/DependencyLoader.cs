using ImportProcessor.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ImportProcessor.WebApi {
	public static class DependencyLoader {

		public static void LoadDependencies( IServiceCollection services ) {

			//domain base
			Domain.DependencyLoader.LoadDependencies( services );

			//services

		}
	}
}
