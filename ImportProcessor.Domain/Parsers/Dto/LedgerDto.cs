using System;
using System.ComponentModel;

namespace ImportProcessor.Domain.Parsers.Dto {

	[ImmutableObject(immutable: true)]
	public sealed class LedgerDto {

		public LedgerDto( 
			int trans,
			LedgerType type, 
			DateTime date, 
			int num, 
			string name,
			string memo, 
			string account, 
			double debit, 
			double credit 
		) {
			Trans = trans;
			Type = type;
			Date = date;
			Num = num;
			Name = name;
			Memo = memo;
			Account = account;
			Debit = debit;
			Credit = credit;
		}

		public int Trans { get; }
		public LedgerType Type { get; }
		public DateTime Date { get; }
		public int Num { get; }
		public string Name { get; }
		public string Memo { get; }
		public string Account { get; }
		public double Debit { get; }
		public double Credit { get; }
	}

}
