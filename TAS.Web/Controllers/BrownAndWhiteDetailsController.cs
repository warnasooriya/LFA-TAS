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
    public class BrownAndWhiteDetailsController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public string AddBrownAndWhiteDetails(JObject data)
        {
            //ILog logger = LogManager.GetLogger(typeof(ApiController));
            //logger.Debug("Add Electronic Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails = data.ToObject<BrownAndWhiteDetailsRequestDto>();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();

            string validateResult = BrownAndWhiteDetailsManagementService.ValidateDealerCurrency(BrownAndWhiteDetails.DealerId, SecurityHelper.Context, AuditHelper.Context);
            if (validateResult == "ok")
            {
                BrownAndWhiteDetailsRequestDto result = BrownAndWhiteDetailsManagementService.AddBrownAndWhiteDetails(BrownAndWhiteDetails, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Electronic Details Added");
                if (result.BrownAndWhiteDetailsInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Electronic Details failed!";
                }
            }
            else
            {
                return validateResult;
            
            }
        }

        [HttpPost]
        public string UpdateBrownAndWhiteDetails(JObject data)
        {
            //ILog logger = LogManager.GetLogger(typeof(ApiController));
            //logger.Debug("Add Electronic Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails = data.ToObject<BrownAndWhiteDetailsRequestDto>();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            string validateResult = BrownAndWhiteDetailsManagementService.ValidateDealerCurrency(BrownAndWhiteDetails.DealerId, SecurityHelper.Context, AuditHelper.Context);
            if (validateResult == "ok")
            {
                BrownAndWhiteDetailsRequestDto result = BrownAndWhiteDetailsManagementService.UpdateBrownAndWhiteDetails(BrownAndWhiteDetails, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Electronic Details Added");
                if (result.BrownAndWhiteDetailsInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Electronic Details failed!";
                }
            }
            else
            {
                return validateResult;
            }
        }

        [HttpPost]
        public object GetBrownAndWhiteDetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
           
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(Guid.Parse(data["Id"].ToString()),
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
                        BrownAndWhiteDetails.status = "Bordx";
                    }
                    else
                    {
                        BrownAndWhiteDetails.status = "Policy";
                    }
                }
            }
            return BrownAndWhiteDetails;
        }
        
        [HttpPost]
        public object GetAllBrownAndWhiteDetails()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();

            BrownAndWhiteAllDetailsResponseDto BrownAndWhiteDetailsData = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            return BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.ToArray();
        }

        [HttpPost]
        public object GetAllItemsForSearchGrid(BnWSearchGridRequestDto BnWSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            return BrownAndWhiteDetailsManagementService.GetAllItemsForSearchGrid(BnWSearchGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }
    }
}
