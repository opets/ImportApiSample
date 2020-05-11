using System.Collections.Generic;
using ImportProcessor.Domain.Parsers.Dto;

namespace ImportProcessor.Domain.Processors {

	public interface ILedgerProcessor {

		void Process( IEnumerable<LedgerDto> items );

	}

}
