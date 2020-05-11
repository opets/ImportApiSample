using System.Collections.Generic;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers {

	public interface IJournalLineProvider {

		IEnumerable<JournalLineDto> GetJournalLines( int pageSize = 0 );

		JournalLineDto GetJournalLine( int tenant, int num, int lineNum );

		int InsertJournalLine( JournalLineDto journalLine );

		void UpdateJournalLine( JournalLineDto journalLine );

		void DeleteJournalLine( int tenant, int num, int lineNum );

	}

}
