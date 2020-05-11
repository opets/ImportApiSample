using System.Collections.Generic;
using ImportProcessor.Domain.Parsers;
using ImportProcessor.Domain.Parsers.Dto;

namespace ImportProcessor.Domain.Processors.Default {

	internal sealed class LedgerProcessor : ILedgerProcessor {

		void ILedgerProcessor.Process( IEnumerable<LedgerDto> items ) {
			foreach( LedgerDto item in items ) {
				
			}
		}

	}

}
