using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using ImportProcessor.Domain.Parsers.Dto;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ImportProcessor.Domain.Parsers.Default {

	internal sealed class LedgerParser : ILedgerParser {

		IEnumerable<LedgerDto> ILedgerParser.Parse( TextReader reader ) {

			var configuration = new Configuration( CultureInfo.InvariantCulture );

			configuration.HasHeaderRecord = true;

			int lineIndex = 0;
			using( var csv = new CsvReader( reader, configuration ) ) {
				csv.Read();
				csv.ReadHeader();

				while( csv.Read() ) {
					lineIndex++;

					string transField = csv.GetField( "Trans #" );
					if( !int.TryParse( transField, NumberStyles.Integer | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out int trans ) ) {
						Log.Logger.Write( LogEventLevel.Error, $"CSV error in line: {lineIndex}. Unable to parse Trans #:'{transField}' to number." );
					}

					string dateField = csv.GetField( "Date" );
					if( !DateTime.TryParse( dateField, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal, out DateTime date ) ) {
						Log.Logger.Write( LogEventLevel.Error, $"CSV error in line: {lineIndex}. Unable to parse Date:'{dateField}' to date." );
					}

					string numField = csv.GetField( "Num" );
					int num = 0;
					if( !string.IsNullOrEmpty( numField ) ) {
						if( !int.TryParse( numField, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num ) ) {
							Log.Logger.Write( LogEventLevel.Error, $"CSV error in line: {lineIndex}. Unable to parse Num:'{numField}' to number." );
						}
					}

					string debitField = csv.GetField( "Debit" );
					if( !double.TryParse( debitField, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double debit ) ) {
						Log.Logger.Write( LogEventLevel.Error, $"CSV error in line: {lineIndex}. Unable to parse Debit:'{debitField}' to decimal." );
					}

					string creditField = csv.GetField( "Credit" );
					if( !double.TryParse( creditField, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double credit ) ) {
						Log.Logger.Write( LogEventLevel.Error, $"CSV error in line: {lineIndex}. Unable to parse Credit:'{creditField}' to decimal." );
					}

					LedgerType type = LedgerTypeHelper.Parse( csv.GetField( "Type" ) );

					var dto = new LedgerDto(
						trans : trans,
						type : type,
						date : date,
						num : num,
						name : csv.GetField( "Name" ),
						memo : csv.GetField( "Memo" ),
						account : csv.GetField( "Account" ),
						debit : debit,
						credit : credit
					);

					yield return dto;
				}
			}
		}

	}

}
