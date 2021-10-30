using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        [Route("RetrivePolicySectionDataForDashboard")]
        public object RetrivePolicySectionDataForDashboard(JObject data)
        {
            object response = null;
            try
            {
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyManagementService = ServiceFactory.GetPolicyManagementService();
                response = policyManagementService.RetrivePolicySectionData(loggedInUserId,
                    SecurityHelper.Context, AuditHelper.Context);
                return response;
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }
    }
}
