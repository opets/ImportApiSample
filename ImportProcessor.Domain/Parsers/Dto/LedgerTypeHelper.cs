namespace ImportProcessor.Domain.Parsers.Dto {
	public static class LedgerTypeHelper {

		public static LedgerType Parse( string str ) {
			switch( str?.ToLower()?.Trim() ) {
				case "check": return LedgerType.Check;
				case "deposit": return LedgerType.Deposit;
				case "generaljournal": return LedgerType.GeneralJournal;
				case "invoice": return LedgerType.Invoice;
				case "payment": return LedgerType.Payment;
				case "transfer": return LedgerType.Transfer;
				default: return LedgerType.None;
			}
		}

	}
}
