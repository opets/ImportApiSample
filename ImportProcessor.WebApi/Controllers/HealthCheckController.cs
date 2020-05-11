using System.Linq;
using ImportProcessor.Domain.Managers;
using Microsoft.AspNetCore.Mvc;

namespace ImportProcessor.WebApi.Controllers {

	public class HealthCheckController : Controller {

		private readonly ITenantManager m_tenantManager;

		public HealthCheckController( ITenantManager tenantManager ) {
			m_tenantManager = tenantManager;
		}

		[AcceptVerbs( "GET" )]
		public IActionResult Test() {

			object obj = new {
				TenantsCount = m_tenantManager.GetTenants(  ).Count()
			};
			return new JsonResult( obj );
		}

	}
}
