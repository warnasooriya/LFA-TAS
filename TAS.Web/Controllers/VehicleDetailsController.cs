
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Services.Entities;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class VehicleDetailsController : ApiController
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public string AddVehicleDetails(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                VehicleDetailsRequestDto VehicleDetails = data.ToObject<VehicleDetailsRequestDto>();
                IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
                string validateResult = VehicleDetailsManagementService.ValidateDealerCurrency(VehicleDetails.DealerId, SecurityHelper.Context, AuditHelper.Context);
                if (validateResult == "ok")
                {
                    VehicleDetailsRequestDto result = VehicleDetailsManagementService.AddVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                    logger.Info("Vehicle Details Added");
                    if (result.VehicleDetailsInsertion)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Add Vehicle Details failed!";
                    }
                }
                else
                {
                    return validateResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Vehicle Details failed!";
            }

        }

        [HttpPost]
        public string UpdateVehicleDetails(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
                VehicleDetailsRequestDto VehicleDetails = data.ToObject<VehicleDetailsRequestDto>();

                string validateResult = VehicleDetailsManagementService.ValidateDealerCurrency(VehicleDetails.DealerId, SecurityHelper.Context, AuditHelper.Context);
                if (validateResult == "ok")
                {
                    VehicleDetailsRequestDto result = VehicleDetailsManagementService.UpdateVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                    logger.Info("Vehicle Details Added");
                    if (result.VehicleDetailsInsertion)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Add Vehicle Details failed!";
                    }
                }
                else
                {
                    return validateResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Vehicle Details failed!";
            }

        }

        [HttpPost]
        public object GetVehicleDetailsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context);
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
           //List<PolicyResponseDto> policies = PolicyManagementService.GetPolicys(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(v => v.ItemId == Guid.Parse(data["Id"].ToString()) && v.Type == "Vehicle");
            List<PolicyResponseDto> policies = PolicyManagementService.GetPolicysByIdAndType(Guid.Parse(data["Id"].ToString()), "Vehicle", SecurityHelper.Context, AuditHelper.Context);
            if (policies.Count > 0)
            {
                foreach (var item in policies)
                {
                    IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
                    if (BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context).BordxDetails.FindAll(b => b.PolicyId == item.Id).Count > 0)
                    {
                        VehicleDetails.status = "Bordx";
                    }
                    else
                    {
                        VehicleDetails.status = "Policy";
                    }
                }
            }
            return VehicleDetails;
        }

        [HttpPost]
        public object GetAllVehicleDetails()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();

            VehicleAllDetailsResponseDto VehicleDetailsData = VehicleDetailsManagementService.GetVehicleAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            return VehicleDetailsData.VehicleAllDetails.ToArray();
        }
        [HttpPost]
        public object GetVehicleDetailsByVin(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsByVin((data["vinNo"].ToString()), SecurityHelper.Context, AuditHelper.Context);

            return VehicleDetails;
        }


        [HttpPost]
        public bool CheckMoreThanOneVehicleByWinNo(JObject data)
        {
            bool isUsed = false;
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            isUsed = VehicleDetailsManagementService.CheckMoreThanOneVehicleByWinNo((data["vinNo"].ToString()), SecurityHelper.Context, AuditHelper.Context);

            return isUsed;
        }
    }
}
