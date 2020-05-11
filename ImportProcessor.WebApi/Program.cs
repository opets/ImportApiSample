using ImportProcessor.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace ImportProcessor.WebApi {
	public class Program {
		public static void Main( string[] args ) {
			CreateWebHostBuilder( args ).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder( string[] args ) =>
			WebHost.CreateDefaultBuilder( args )
				.ConfigureLogging( ( context, builder ) => {
					builder.AddSerilog();
				} )
				.UseStartup<Startup>();
	}
}
