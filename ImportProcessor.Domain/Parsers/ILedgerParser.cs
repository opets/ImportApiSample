using System.Collections.Generic;
using System.IO;
using ImportProcessor.Domain.Parsers.Dto;

namespace ImportProcessor.Domain.Parsers {

	public interface ILedgerParser {

		IEnumerable<LedgerDto> Parse( TextReader reader );

	}

}
