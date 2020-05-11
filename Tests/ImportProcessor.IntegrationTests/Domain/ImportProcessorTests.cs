using System.IO;
using CsvHelper;
using ImportProcessor.Domain.Processors;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImportProcessor.IntegrationTests.Domain {

	public class ImportProcessorTests {

		private readonly IMainProcessor m_sut;

		public ImportProcessorTests() {
			m_sut = DependencyLoader.ServiceLocator.GetService<IMainProcessor>();
		}

		[Fact]
		public void ParseCSV_Pass() {
			var csvPath = Path.Combine( Directory.GetCurrentDirectory(), "Resources", "ledgerdec.csv" );

			using( var reader = new StreamReader( csvPath ) )
			using( var csv = new CsvReader( reader ) ) {
				var records = csv.GetRecords<string>();
			}

		}

	}
}
