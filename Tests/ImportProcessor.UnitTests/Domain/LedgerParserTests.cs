using System.IO;
using System.Linq;
using ImportProcessor.Domain.Parsers;
using ImportProcessor.Domain.Parsers.Default;
using ImportProcessor.Domain.Parsers.Dto;
using Xunit;

namespace ImportProcessor.UnitTests.Domain {
	public class LedgerParserTests {

		private readonly ILedgerParser m_sut;

		public LedgerParserTests() {
			m_sut = new LedgerParser( );
		}

		[Fact]
		public void Parse_Payment_Pass() {
			const string csv = 
			  @",Trans #,,Type,,Date,,Num,,Name,,Memo,,Account,,Debit,,Credit
				,""11,165"",,Payment,,01/04/2014,,,,Customer 1,,,,Undeposited Funds,,55485.00,,0.00
				,,,,,,,,,Customer 1,,,,Accts. Rec.,,0.00,,55485.00
				,,,,,,,,,,,,,,,55485.00,,55485.00";

			TextReader reader = new StringReader( csv);

			LedgerDto[] actual  = m_sut.Parse( reader ).ToArray();

			Assert.NotNull( actual );
			Assert.Equal( 3, actual.Count() );

			Assert.Equal( 11165, actual[0].Trans );
			Assert.Equal( LedgerType.Payment, actual[0].Type );
			Assert.Equal( "2014-01-04T00:00:00.0000000", actual[0].Date.ToString( "O" ) );
			Assert.Equal( "Customer 1", actual[0].Name );
			Assert.Equal( "Customer 1", actual[1].Name );
			Assert.Equal( "Undeposited Funds", actual[0].Account );
			Assert.Equal( "Accts. Rec.", actual[1].Account );
			Assert.Equal( 55485, actual[0].Debit );
			Assert.Equal( 0, actual[1].Debit );
			Assert.Equal( 55485, actual[2].Debit );
			Assert.Equal( 0, actual[0].Credit );
			Assert.Equal( 55485, actual[1].Credit );
			Assert.Equal( 55485, actual[2].Credit );
		}


		[Fact]
		public void Parse_Invoice_Pass() {
			const string csv =
				@",Trans #,,Type,,Date,,Num,,Name,,Memo,,Account,,Debit,,Credit
				,""11,174"",,Invoice,,01/14/2014,,5001,,Customer 6,,,,Accts. Rec.,,28852.20,,0.00
				,,,,,,,,,Customer 6,,Target provided note 4,,1099,,0.00,,28852.20
				 ,,,,,,,,,,,,,,,28852.20,,28852.20";

			TextReader reader = new StringReader( csv );

			LedgerDto[] actual = m_sut.Parse( reader ).ToArray();

			Assert.NotNull( actual );
			Assert.Equal( 3, actual.Count() );

			Assert.Equal( 11174, actual[0].Trans );
			Assert.Equal( LedgerType.Invoice, actual[0].Type );
			Assert.Equal( "2014-01-14T00:00:00.0000000", actual[0].Date.ToString( "O" ) );
			Assert.Equal( 5001, actual[0].Num );
			Assert.Equal( "Customer 6", actual[0].Name );
			Assert.Equal( "Customer 6", actual[1].Name );
			Assert.Equal( "", actual[0].Memo );
			Assert.Equal( "Target provided note 4", actual[1].Memo );
			Assert.Equal( "Accts. Rec.", actual[0].Account );
			Assert.Equal( "1099", actual[1].Account );
			Assert.Equal( 28852.20, actual[0].Debit );
			Assert.Equal( 0, actual[1].Debit );
			Assert.Equal( 28852.20, actual[2].Debit );
			Assert.Equal( 0, actual[0].Credit );
			Assert.Equal( 28852.20, actual[1].Credit );
			Assert.Equal( 28852.20, actual[2].Credit );
		}
	}
}
