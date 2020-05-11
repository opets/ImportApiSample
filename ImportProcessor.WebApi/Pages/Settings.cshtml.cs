using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImportProcessor.WebApi.Pages {

	public class SettingsModel : PageModel {

		private readonly IAdminSettingsService m_adminSettingsService;

		public SettingsModel( IAdminSettingsService adminSettingsService ) {
			m_adminSettingsService = adminSettingsService;
		}

		[BindProperty]
		public string LatestVersion { get; set; }

		[BindProperty]
		public string LatestVersionSource { get; set; }

		[Url]
		[BindProperty]
		public string PaymentUrl { get; set; }

		[BindProperty]
		public string PrinterPath { get; set; }

		[BindProperty]
		public string SalesforceClientId { get; set; }

		[BindProperty]
		public string SalesforceClientSecret { get; set; }

		[Url]
		[BindProperty]
		public string SalesforceRedirectUrl { get; set; }

		[Url]
		[BindProperty]
		public string SalesforceUrl { get; set; }

		[BindProperty]
		public string SmartsheetToken { get; set; }

		[Url]
		[BindProperty]
		public string SmartsheetUrl { get; set; }

		[BindProperty]
		public string EmailTemplate { get; set; }

		[BindProperty]
		public string SalesforceRecordTypeId { get; set; }

		public string ErrorLog { get; set; }

		public void OnGet() {
			ErrorLog = string.Empty;
			LatestVersion = GetValueSafe( () => m_adminSettingsService.LatestVersion, "LatestVersion" );
			LatestVersionSource = GetValueSafe( () => m_adminSettingsService.LatestVersionSource, "LatestVersionSource" );
			PaymentUrl = GetValueSafe( () => m_adminSettingsService.PaymentUrl, "PaymentUrl" );
			PrinterPath = GetValueSafe( () => m_adminSettingsService.PrinterPath, "PrinterPath" );
			SalesforceClientId = GetValueSafe( () => m_adminSettingsService.SalesforceClientId, "SalesforceClientId" );
			SalesforceClientSecret = GetValueSafe( () => m_adminSettingsService.SalesforceClientSecret, "SalesforceClientSecret" );
			SalesforceRedirectUrl = GetValueSafe( () => m_adminSettingsService.SalesforceRedirectUrl, "SalesforceRedirectUrl" );
			SalesforceUrl = GetValueSafe( () => m_adminSettingsService.SalesforceUrl, "SalesforceUrl" );
			SmartsheetToken = GetValueSafe( () => m_adminSettingsService.SmartsheetToken, "SmartsheetToken" );
			SmartsheetUrl = GetValueSafe( () => m_adminSettingsService.SmartsheetUrl, "SmartsheetUrl" );
			EmailTemplate = GetValueSafe( () => m_adminSettingsService.EmailTemplate, "EmailTemplate" );
			SalesforceRecordTypeId = GetValueSafe( () => m_adminSettingsService.SalesforceRecordTypeId, "SalesforceRecordTypeId" );
		}

		public IActionResult OnPostAsync( string setDefault ) {

			if( !string.IsNullOrEmpty( setDefault ) ) {
				LatestVersion = "3.0.0.0";
				LatestVersionSource = "D:\\Work\\Temp\\3.zip";
				PaymentUrl = "https://payments.integrapay.com.au/RTP/Payment.aspx?b=9de7caed-d7cd-4828-80be-4e19ab1bf301";
				PrinterPath = "\\\\cri - prn - 01\\entro - vic - despatch";
				SalesforceClientId = "3MVG9Se4BnchkASmtngzNh_R0lU3MVimKPn1cgQu5nWVo6NPkciW.qcts9tHMd0aaZ1_4PBQ.ndcK0C5XJmPA";
				SalesforceClientSecret = "4739437617864260065";
				SalesforceRedirectUrl = "https://test.salesforce.com/services/oauth2/callback";
				SalesforceUrl = "https://test.salesforce.com/services/oauth2/token";
				SmartsheetToken = "Bearer 5jvrz4xndqc5ait1ntr74naxby";
				SmartsheetUrl = "https://api.smartsheet.com/2.0/sheets/1058465297786756";
				SalesforceRecordTypeId = "0120K000000neReQAI";
				EmailTemplate = @"C:\";
			}

			var settings = new Dictionary<string, string> {
				{ "LatestVersion", LatestVersion },
				{ "LatestVersionSource", LatestVersionSource },
				{ "PaymentUrl", PaymentUrl },
				{ "PrinterPath", PrinterPath },
				{ "SalesforceClientId", SalesforceClientId },
				{ "SalesforceClientSecret", SalesforceClientSecret },
				{ "SalesforceRedirectUrl", SalesforceRedirectUrl },
				{ "SalesforceUrl", SalesforceUrl },
				{ "SmartsheetToken", SmartsheetToken },
				{ "SmartsheetUrl", SmartsheetUrl },
				{ "EmailTemplate", EmailTemplate },
				{ "SalesforceRecordTypeId", SalesforceRecordTypeId }
			};

			m_adminSettingsService.SaveSettings( settings );

			return RedirectToPage();
		}

		private string GetValueSafe( Func<string> getFunc, string propertyName ) {
			try {

				return getFunc();

			} catch( Exception e ) {
				ErrorLog += $"{propertyName}: \t {e.Message}{Environment.NewLine}";
				if( e.InnerException != null ) {
					ErrorLog += $"(Inner: {e.InnerException.Message}){Environment.NewLine}";
				}
				return string.Empty;
			}
		}

	}
}