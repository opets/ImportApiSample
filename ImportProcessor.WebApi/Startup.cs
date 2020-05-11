using ImportProcessor.Data;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Common.Default;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ImportProcessor.WebApi {

	public class Startup {
		public Startup( IConfiguration configuration ) {
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration( configuration ).CreateLogger();

			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services ) {
			services.Configure<CookiePolicyOptions>( options => {
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			} );

			var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
			services.AddSingleton<IDataRepository>( a => new DataRepository( connectionString ) );
			DependencyLoader.LoadDependencies( services );

			services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IHostingEnvironment env ) {
			if( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler( "/Error" );
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles()
				.UseMvc( routes => {
					routes.MapRoute( name: "default", template: "{controller=JavaScript}/{action=Basics}" );
				} );
			app.UseCookiePolicy();

			app.UseMvc();
		}
	}
}
