using System.Collections.Generic;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers {

	public interface IJournalEntryProvider {

		IEnumerable<JournalEntryDto> GetJournalEntries( int pageSize = 0 );

		JournalEntryDto GetJournalEntry( int tenant, int num );

		int InsertJournalEntry( JournalEntryDto journalEntry );

		void UpdateJournalEntry( JournalEntryDto journalEntry );

		void DeleteJournalEntry( int tenant, int num );

	}

}
