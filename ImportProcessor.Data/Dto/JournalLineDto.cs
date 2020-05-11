using System;
using System.ComponentModel;

namespace ImportProcessor.Data.Dto {

	[ImmutableObject( immutable: true )]
	public sealed class JournalLineDto {

		public int Tenant { get; }
		public DateTime Date { get; }
		public int Num { get; }
		public int LineNum { get; }

		public long Amount { get; }
		public int Account { get; }
		public string Description { get; }

		public JournalLineDto( int tenant, DateTime date, int num, int lineNum, long amount, int account, string description ) {
			Tenant = tenant;
			Date = date;
			Num = num;
			LineNum = lineNum;
			Amount = amount;
			Account = account;
			Description = description;
		}

	}
}
