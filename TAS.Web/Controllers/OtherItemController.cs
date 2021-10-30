using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class OtherItemController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public object GetAllItemsForSearchGrid(OtherItemSearchGridRequestDto OtherItemSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IOtherItemManagementService OtherItemManagementService = ServiceFactory.GetOtherItemManagementService();
            return OtherItemManagementService.GetAllItemsForSearchGrid(OtherItemSearchGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetOtherItemDetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid OtherItemId = Guid.Parse(data["Id"].ToString());
            IOtherItemManagementService OtherItemManagementService = ServiceFactory.GetOtherItemManagementService();
            return OtherItemManagementService.GetOtherItemDetailsById(OtherItemId,
                SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetOtherDetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid OtherItemId = Guid.Parse(data["Id"].ToString());
            IOtherItemManagementService OtherItemManagementService = ServiceFactory.GetOtherItemManagementService();
            OtherItemDetailsResponseDto OtherItemDetails = OtherItemManagementService.GetOtherDetailsById(OtherItemId,
                SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            List<PolicyResponseDto> policies = PolicyManagementService.GetPolicys(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(v => v.ItemId == Guid.Parse(data["Id"].ToString()) && v.Type != "Vehicle");
            if (policies.Count > 0)
            {
                foreach (var item in policies)
                {
                    IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
                    if (BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context).BordxDetails.FindAll(b => b.PolicyId == item.Id).Count > 0)
                    {
                        OtherItemDetails.status = "Bordx";
                    }
                    else
                    {
                        OtherItemDetails.status = "Policy";
                    }
                }
            }

            return OtherItemDetails;

        }
        
    }
}
