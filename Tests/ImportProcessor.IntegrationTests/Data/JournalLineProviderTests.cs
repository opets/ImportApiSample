using System;
using System.Collections.Generic;
using System.Linq;
using ImportProcessor.Data.Dto;
using ImportProcessor.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImportProcessor.IntegrationTests.Data {

	public class JournalLineProviderTests : IDisposable {

		private readonly int m_tenantId;
		private readonly int m_accountId;
		private readonly DateTime m_journalDate;
		private readonly int m_journalNum;

		private readonly IJournalLineProvider m_sut;
		private readonly IAccountProvider m_accountProvider;
		private readonly ITenantProvider m_tenantProvider;
		private readonly IJournalEntryProvider m_journalEntryProvider;

		public JournalLineProviderTests() {
			m_sut = DependencyLoader.ServiceLocator.GetService<IJournalLineProvider>();
			m_tenantProvider = DependencyLoader.ServiceLocator.GetService<ITenantProvider>();
			m_accountProvider = DependencyLoader.ServiceLocator.GetService<IAccountProvider>();
			m_journalEntryProvider = DependencyLoader.ServiceLocator.GetService<IJournalEntryProvider>();

			m_journalDate = new DateTime( 2019, 05, 11 );
			m_tenantId = m_tenantProvider.InsertTenant( new TenantDto( 0, $"test tenant {Guid.NewGuid()}" ) );
			m_accountId = m_accountProvider.InsertAccount( new AccountDto( m_tenantId, 0, "test account", 111 ) );
			m_journalNum = m_journalEntryProvider.InsertJournalEntry( new JournalEntryDto( 
				m_tenantId, m_journalDate, 0, "USD", "test-doc-num", "test-doc-notes"
			) );
		}

		[Fact]
		public void CreateUpdateDelete_Pass() {
			string description = $"test{Guid.NewGuid().ToString().Substring( 12 )}";
			string description2 = $"{description}2";

			//Create
			var dtoToInsert = new JournalLineDto( m_tenantId, m_journalDate, m_journalNum, 0, 123, m_accountId, description );
			int lineNum = m_sut.InsertJournalLine( dtoToInsert );
			Assert.True( lineNum > 0, "failed on InsertJournalLine" );

			//Get
			JournalLineDto journalLine = m_sut.GetJournalLine( m_tenantId, m_journalNum, lineNum );
			Assert.NotNull( journalLine );
			Assert.Equal( m_tenantId, journalLine.Tenant );
			Assert.Equal( m_accountId, journalLine.Account );
			Assert.Equal( m_journalDate, journalLine.Date );
			Assert.Equal( m_journalNum, journalLine.Num );
			Assert.Equal( 123, journalLine.Amount );
			Assert.Equal( description, journalLine.Description );

			try {

				//Update
				var dtoToUpdate = new JournalLineDto( m_tenantId, m_journalDate, m_journalNum, lineNum, 321, m_accountId, description2 );
				m_sut.UpdateJournalLine( dtoToUpdate );
				JournalLineDto updatedJournalLine = m_sut.GetJournalLine( m_tenantId, m_journalNum, lineNum );
				Assert.NotNull( updatedJournalLine );
				Assert.Equal( m_tenantId, updatedJournalLine.Tenant );
				Assert.Equal( m_accountId, updatedJournalLine.Account );
				Assert.Equal( m_journalDate, updatedJournalLine.Date );
				Assert.Equal( m_journalNum, updatedJournalLine.Num );
				Assert.Equal( 321, updatedJournalLine.Amount );
				Assert.Equal( description2, updatedJournalLine.Description );

			}
			finally {

				//Cleanup
				m_sut.DeleteJournalLine( m_tenantId, m_journalNum, lineNum );
				journalLine = m_sut.GetJournalLine( m_tenantId, m_journalNum, lineNum );
				Assert.Null( journalLine );
			}
		}

		[Fact]
		public void GetJournalLines_Paging_Pass() {
			string description = $"test{Guid.NewGuid().ToString().Substring( 12 )}";

			//Create
			int[] ids = {
				m_sut.InsertJournalLine( new JournalLineDto(
					m_tenantId, m_journalDate, m_journalNum, 0, 321, m_accountId, $"{description} 1"
				)),
				m_sut.InsertJournalLine( new JournalLineDto(
					m_tenantId, m_journalDate, m_journalNum, 0, 321, m_accountId, $"{description} 2"
				)),
				m_sut.InsertJournalLine( new JournalLineDto(
					m_tenantId, m_journalDate, m_journalNum, 0, 321, m_accountId, $"{description} 3"
				))
			};

			try {

				//Get
				IEnumerable<JournalLineDto> twoJournalLines = m_sut.GetJournalLines( 2 );
				IEnumerable<JournalLineDto> allJournalLines = m_sut.GetJournalLines()
					.Where( x => x.Description.StartsWith( description ) );

				Assert.Equal( 2, twoJournalLines.Count() );
				Assert.Equal( 3, allJournalLines.Count() );

			}
			finally {

				//Cleanup
				ids.ToList().ForEach( id => m_sut.DeleteJournalLine( m_tenantId, m_journalNum, id ) );
			}
		}

		public void Dispose() {
			m_journalEntryProvider.DeleteJournalEntry( m_tenantId, m_journalNum );
			m_tenantProvider.DeleteTenant( m_tenantId );
		}

	}
}
