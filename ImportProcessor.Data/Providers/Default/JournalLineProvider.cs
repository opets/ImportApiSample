using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Common;
using ImportProcessor.Data.Dto;

namespace ImportProcessor.Data.Providers.Default {

	internal class JournalLineProvider : IJournalLineProvider {

		private readonly IDataRepository m_dataRepository;

		public JournalLineProvider( IDataRepository dataRepository ) {
			m_dataRepository = dataRepository;
		}

		IEnumerable<JournalLineDto> IJournalLineProvider.GetJournalLines( int pageSize ) {
			string sql = $@"
				SELECT 
					tenant,
					tx_date as date,
					tx_num as num,
					line_num as lineNum,
					amount,
					`account`,
					description
				FROM journal_line
				{( pageSize > 0 ? $"LIMIT {pageSize}" : string.Empty )};
			";

			IEnumerable<JournalLineDto> journalLines = m_dataRepository.Query<JournalLineDto>(
				sql,
				new { }
			);

			return journalLines;
		}

		JournalLineDto IJournalLineProvider.GetJournalLine( int tenant, int num, int lineNum ) {
			const string sql = @"
				SELECT 
					tenant,
					tx_date as date,
					tx_num as num,
					line_num as lineNum,
					amount,
					`account`,
					description
				FROM journal_line
				WHERE tenant = @tenant 
					AND tx_num = @num
					AND line_num = @lineNum;
			";
			JournalLineDto journalLine = m_dataRepository.Query<JournalLineDto>(
				sql,
				new {
					tenant,
					num,
					lineNum
				}
			).FirstOrDefault();

			return journalLine;
		}

		int IJournalLineProvider.InsertJournalLine( JournalLineDto journalLine ) {
			const string sql = @"
				SET @newId := (SELECT coalesce(MAX(line_num), 0) + 1 FROM journal_line WHERE tenant = @tenant AND tx_num = @num);

				INSERT INTO journal_line (tenant, tx_date, tx_num, line_num, amount, `account`, description)
				VALUES (@tenant, @date, @num, @newId, @amount, @account, @description);

				SELECT @newId;
			";
			var res = m_dataRepository.ExecuteScalar<int>(
				sql,
				journalLine
			);

			return res;
		}


		void IJournalLineProvider.UpdateJournalLine( JournalLineDto journalLine ) {
			const string sql = @"
				UPDATE journal_line SET
					tx_date = @date,
					amount = @amount,
					`account` = @account,
					description = @description
				WHERE tenant = @tenant 
					AND tx_num = @num
					AND line_num = @lineNum;
			";
			m_dataRepository.Execute(
				sql,
				journalLine
			);
		}

		void IJournalLineProvider.DeleteJournalLine( int tenant, int num, int lineNum ) {
			const string sql = @"
				DELETE FROM journal_line
				WHERE tenant = @tenant 
					AND tx_num = @num
					AND line_num = @lineNum;
			";
			m_dataRepository.Execute(
				sql,
				new {
					tenant,
					num,
					lineNum
				}
			);
		}

	}
}
