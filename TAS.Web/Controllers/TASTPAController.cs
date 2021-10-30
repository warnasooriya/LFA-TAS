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

namespace TAS.Web.Controllers
{
    public class TASTPAController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        public object GetAllTPAs()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            TASTPAsResponseDto TPAData = TPAManagementService.GetAllTPAs(
            SecurityHelper.Context,
            AuditHelper.Context);
            return TPAData.TPAs.ToArray();
        }

        [HttpPost]
        public object GetTPADetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            TASTPAsResponseDto TPAData = TPAManagementService.GetTPADetailsByTPAId(
            SecurityHelper.Context,
            AuditHelper.Context,
            Guid.Parse(data["tpaId"].ToString()));
            return TPAData.TPAs.ToArray();
        }

        [HttpPost]
        public object SaveTPA(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TASTPARequestDto TPAData = new TASTPARequestDto();
            TPAData.Address = data["Address"].ToString();
            TPAData.DiscountDescription = data["DiscountDescription"].ToString();
            TPAData.Name = data["Name"].ToString();
            TPAData.TelNumber = data["TelNumber"].ToString();
            TPAData.Banner = data["Banner"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Banner"].ToString());
            TPAData.Logo = data["Logo"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Logo"].ToString());
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            bool status = TPAManagementService.SaveTPA(TPAData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }

        [HttpPost]
        public object UpdateTPA(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TASTPARequestDto TPAData = new TASTPARequestDto();
            TPAData.Id = Guid.Parse(data["Id"].ToString());
            TPAData.Address = data["Address"].ToString();
            TPAData.DiscountDescription = data["DiscountDescription"].ToString();
            TPAData.Name = data["Name"].ToString();
            TPAData.TelNumber = data["TelNumber"].ToString();
            TPAData.Banner = data["Banner"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Banner"].ToString());
            TPAData.Logo = data["Logo"].ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.Empty : Guid.Parse(data["Logo"].ToString());
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            bool status = TPAManagementService.UpdateTPA(TPAData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }

    }
}
