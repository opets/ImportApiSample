using ImportProcessor.Data.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ImportProcessor.WebApi.Controllers {

	public class ImportController : Controller {

		private readonly ITenantProvider m_tenantProvider;


		public ImportController( ITenantProvider tenantProvider ) {
			m_tenantProvider = tenantProvider;
		}

		[AcceptVerbs( "POST" )]
		public IActionResult Import( byte[] csvBody ) {


			return new OkResult();
		}


	}
}
