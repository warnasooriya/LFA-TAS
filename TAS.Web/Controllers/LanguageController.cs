using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class LanguageController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpGet]
        public object GetAllLanguages()
        {
            try
            {
                if (Request.Headers.Authorization.ToString() != null)
                {
                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    ISystemLanguageManagementService LanguageManagementService = ServiceFactory.GetSystemLanguageManagementService();
                    return LanguageManagementService.GetAllLanguages(SecurityHelper.Context, AuditHelper.Context);
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
    }
}
