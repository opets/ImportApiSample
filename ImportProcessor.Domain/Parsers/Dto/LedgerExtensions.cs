namespace ImportProcessor.Domain.Parsers.Dto {

	internal static class LedgerExtensions {

		internal static bool IsFirstLine( this LedgerDto dto )
			=> dto.Trans > 0;

	}
}
