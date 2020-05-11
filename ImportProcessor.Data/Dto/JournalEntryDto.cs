using System;
using System.ComponentModel;

namespace ImportProcessor.Data.Dto {

	[ImmutableObject( immutable: true )]
	public sealed class JournalEntryDto {

		public int Tenant { get; }
		public DateTime Date { get; }
		public int Num { get; }

		public string Currency { get; }
		public string DocumentNumber { get; }
		public string PrivateNote { get; }

		public JournalEntryDto( int tenant, DateTime date, int num, string currency, string documentNumber, string privateNote ) {
			Tenant = tenant;
			Date = date;
			Num = num;
			Currency = currency;
			DocumentNumber = documentNumber;
			PrivateNote = privateNote;
		}

	}
}
