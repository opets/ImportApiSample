using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers.Default {

	internal class JournalEntryProvider : IJournalEntryProvider {

		private readonly IDataRepository m_dataRepository;

		public JournalEntryProvider( IDataRepository dataRepository ) {
			m_dataRepository = dataRepository;
		}

		IEnumerable<JournalEntryDto> IJournalEntryProvider.GetJournalEntries( int pageSize ) {
			string sql = $@"
				SELECT 
					tenant,
					tx_date as date,
					tx_num as num,
					currency,
					doc_number as documentNumber,
					private_note as privateNote
				FROM journal_entry
				{( pageSize > 0 ? $"LIMIT {pageSize}" : string.Empty )};
			";

			IEnumerable<JournalEntryDto> journalEntries = m_dataRepository.Query<JournalEntryDto>(
				sql,
				new { }
			);

			return journalEntries;
		}

		JournalEntryDto IJournalEntryProvider.GetJournalEntry( int tenant, int num ) {
			const string sql = @"
				SELECT 
					tenant,
					tx_date as date,
					tx_num as num,
					currency,
					doc_number as documentNumber,
					private_note as privateNote
				FROM journal_entry
				WHERE tenant = @tenant 
					AND tx_num = @num;
			";
			JournalEntryDto journalEntry = m_dataRepository.Query<JournalEntryDto>(
				sql,
				new {
					tenant,
					num
				}
			).FirstOrDefault();

			return journalEntry;
		}

		int IJournalEntryProvider.InsertJournalEntry( JournalEntryDto journalEntry ) {
			const string sql = @"
				SET @newId := (SELECT coalesce(MAX(tx_num), 0) + 1 FROM journal_entry WHERE tenant = @tenant);

				INSERT INTO journal_entry (tenant, tx_date, tx_num, currency, doc_number, private_note)
				VALUES (@tenant, @date, @newId, @currency, @documentNumber, @privateNote);

				SELECT @newId;
			";
			var res = m_dataRepository.ExecuteScalar<int>(
				sql,
				journalEntry
			);

			return res;
		}


		void IJournalEntryProvider.UpdateJournalEntry( JournalEntryDto journalEntry ) {
			const string sql = @"
				UPDATE journal_entry SET
					tx_date = @date,
					currency = @currency,
					doc_number = @documentNumber,
					private_note = @privateNote
				WHERE tenant = @tenant 
					AND tx_num = @num;
			";
			m_dataRepository.Execute(
				sql,
				journalEntry
			);
		}

		void IJournalEntryProvider.DeleteJournalEntry( int tenant, int num ) {
			const string sql = @"
				DELETE FROM journal_entry
				WHERE tenant = @tenant 
					AND tx_num = @num;
			";
			m_dataRepository.Execute(
				sql,
				new {
					tenant,
					num
				}
			);
		}

	}
}
