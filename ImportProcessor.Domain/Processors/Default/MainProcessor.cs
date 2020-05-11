using System.Collections.Generic;
using System.IO;
using ImportProcessor.Domain.Parsers;
using ImportProcessor.Domain.Parsers.Dto;

namespace ImportProcessor.Domain.Processors.Default {

	internal sealed class MainProcessor : IMainProcessor {

		private readonly ILedgerProcessor m_ledgerProcessor;
		private readonly ILedgerParser m_ledgerParser;

		public MainProcessor(
			ILedgerProcessor ledgerProcessor,
			ILedgerParser ledgerParser 
		) {
			m_ledgerProcessor = ledgerProcessor;
			m_ledgerParser = ledgerParser;

		}
		void IMainProcessor.Import( TextReader reader ) {

			IEnumerable<LedgerDto> items = m_ledgerParser.Parse( reader );
			m_ledgerProcessor.Process( items );

		}

	}

}
