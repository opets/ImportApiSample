using System;
using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImportProcessor.IntegrationTests.Data {

	public class JournalEntryProviderTests : IDisposable {

		private readonly int m_tenantId;
		private readonly IJournalEntryProvider m_sut;
		private readonly ITenantProvider m_tenantProvider;

		public JournalEntryProviderTests() {
			m_sut = DependencyLoader.ServiceLocator.GetService<IJournalEntryProvider>();
			m_tenantProvider = DependencyLoader.ServiceLocator.GetService<ITenantProvider>();
			m_tenantId  = m_tenantProvider.InsertTenant( new TenantDto( 0, $"test tenant {Guid.NewGuid()}" ) );
		}

		[Fact]
		public void CreateUpdateDelete_Pass() {
			string documentNumber = $"test{Guid.NewGuid().ToString().Substring( 12 )}";
			string documentNumber2 = $"{documentNumber}2";
			var date = new DateTime( 2019, 05, 11 );

			//Create
			var dtoToInsert = new JournalEntryDto( m_tenantId, date, 0, "USD", documentNumber, $"{documentNumber} notes." );
			int num = m_sut.InsertJournalEntry( dtoToInsert );
			Assert.True( num > 0, "failed on InsertJournalEntry" );

			//Get
			JournalEntryDto journalEntry = m_sut.GetJournalEntry( m_tenantId, num );
			Assert.NotNull( journalEntry );
			Assert.Equal( m_tenantId, journalEntry.Tenant );
			Assert.Equal( date, journalEntry.Date );
			Assert.Equal( "USD", journalEntry.Currency );
			Assert.Equal( documentNumber, journalEntry.DocumentNumber );
			Assert.Equal( $"{documentNumber} notes.", journalEntry.PrivateNote );

			try {

				//Update
				var dtoToUpdate = new JournalEntryDto( m_tenantId, date, num, "EUR", documentNumber2, $"{documentNumber2} notes." );
				m_sut.UpdateJournalEntry( dtoToUpdate );
				JournalEntryDto updatedJournalEntry = m_sut.GetJournalEntry( m_tenantId, num );
				Assert.NotNull( updatedJournalEntry );
				Assert.Equal( m_tenantId, updatedJournalEntry.Tenant );
				Assert.Equal( date, updatedJournalEntry.Date );
				Assert.Equal( "EUR", updatedJournalEntry.Currency );
				Assert.Equal( documentNumber2, updatedJournalEntry.DocumentNumber );
				Assert.Equal( $"{documentNumber2} notes.", updatedJournalEntry.PrivateNote );

			}
			finally {

				//Cleanup
				m_sut.DeleteJournalEntry( m_tenantId, num );
				journalEntry = m_sut.GetJournalEntry( m_tenantId, num );
				Assert.Null( journalEntry );
			}
		}

		[Fact]
		public void GetJournalEntrys_Paging_Pass() {
			string documentNumber = $"test{Guid.NewGuid().ToString().Substring( 12 )}";
			var date = new DateTime( 2019, 06, 18 );

			//Create
			int[] ids = {
				m_sut.InsertJournalEntry( new JournalEntryDto(
					m_tenantId, date, 0, "A", $"{documentNumber} 1", $"{documentNumber} 1 notes."
				)),
				m_sut.InsertJournalEntry( new JournalEntryDto(
					m_tenantId, date, 0, "B", $"{documentNumber} 2", $"{documentNumber} 2 notes."
				)),
				m_sut.InsertJournalEntry( new JournalEntryDto(
					m_tenantId, date, 0, "C", $"{documentNumber} 3", $"{documentNumber} 3 notes."
				))
			};

			try {

				//Get
				IEnumerable<JournalEntryDto> twoJournalEntries = m_sut.GetJournalEntries( 2 );
				IEnumerable<JournalEntryDto> allJournalEntries = m_sut.GetJournalEntries()
					.Where( x => x.DocumentNumber.StartsWith( documentNumber ) );

				Assert.Equal( 2, twoJournalEntries.Count() );
				Assert.Equal( 3, allJournalEntries.Count() );

			}
			finally {

				//Cleanup
				ids.ToList().ForEach( id => m_sut.DeleteJournalEntry( m_tenantId, id ) );
			}
		}

		public void Dispose() {
			m_tenantProvider.DeleteTenant( m_tenantId );
		}

	}
}
