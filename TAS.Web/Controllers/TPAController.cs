using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
using System.Reflection;

namespace TAS.Web.Controllers
{
    public class TPAController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet] [AcceptVerbs("GET", "POST")]
        public object GetAllTPAs()
        {
            try
            {
                if (Request.Headers.Authorization.ToString() != null)
                {

                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                    ITPAManagementService TPAManagementService = ServiceFactory.GetTPAManagementService();

                    TPAsResponseDto TPAData = TPAManagementService.GetAllTPAs(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                    return TPAData.TPAs.ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetTPADetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITPAManagementService TPAManagementService = ServiceFactory.GetTPAManagementService();
            TPAsResponseDto TPAData = TPAManagementService.GetTPADetailsByTPAId(
            SecurityHelper.Context,
            AuditHelper.Context,
            Guid.Parse(data["tpaId"].ToString()));
            return TPAData.TPAs.ToArray();
        }

        [HttpPost]
        public object SaveTPA(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TPARequestDto TPAData = new TPARequestDto();
            TPAData.Address = data["Address"].ToString();
            TPAData.DiscountDescription = data["DiscountDescription"].ToString();
            TPAData.Name = data["Name"].ToString();
            TPAData.TelNumber = data["TelNumber"].ToString();
            TPAData.Banner = data["Banner"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner"].ToString());
            TPAData.Banner2 = data["Banner2"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner2"].ToString());
            TPAData.Banner3 = data["Banner3"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner3"].ToString());
            TPAData.Banner4 = data["Banner4"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner4"].ToString());
            TPAData.Banner5 = data["Banner5"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner5"].ToString());
            TPAData.Logo = data["Logo"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Logo"].ToString());
            ITPAManagementService TPAManagementService = ServiceFactory.GetTPAManagementService();
            bool status = TPAManagementService.SaveTPA(TPAData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }

        [HttpPost]
        public object UpdateTPA(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TPARequestDto TPAData = new TPARequestDto();
            TPAData.Id = Guid.Parse(data["Id"].ToString());
            TPAData.Address = data["Address"].ToString();
            TPAData.DiscountDescription = data["DiscountDescription"].ToString();
            TPAData.Name = data["Name"].ToString();
            TPAData.TelNumber = data["TelNumber"].ToString();
            TPAData.TpaCode = data["tpaCode"].ToString();
            TPAData.Banner = data["Banner"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner"].ToString());
            TPAData.Banner2 = data["Banner2"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner2"].ToString());
            TPAData.Banner3 = data["Banner3"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner3"].ToString());
            TPAData.Banner4 = data["Banner4"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner4"].ToString());
            TPAData.Banner5 = data["Banner5"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner5"].ToString());
            TPAData.Logo = data["Logo"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Logo"].ToString());
            ITPAManagementService TPAManagementService = ServiceFactory.GetTPAManagementService();
            bool status = TPAManagementService.UpdateTPA(TPAData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }
        
    }
}
