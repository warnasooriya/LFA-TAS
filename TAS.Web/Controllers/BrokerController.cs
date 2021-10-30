using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class BrokerController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        

        [HttpPost]
        public object SaveBroker(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            BrokerRequestDto BrokerData = new BrokerRequestDto();
            BrokerData.Code = data["Code"].ToString();
            BrokerData.Name = data["Name"].ToString();
            BrokerData.BrokerStatus = data["Status"].ToString();
            BrokerData.TelNumber = data["TelNumber"].ToString();
            BrokerData.CountryId = data["CountryId"].ToString() == "" ? Guid.Empty : Guid.Parse(data["CountryId"].ToString());
            BrokerData.Address = data["Address"].ToString();

            IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();
            bool status = BrokerManagementService.SaveBroker(BrokerData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }

        [HttpGet]
        [AcceptVerbs("GET", "POST")]
        public object GetAllBrokers()
        {
            try
            {
                if (Request.Headers.Authorization.ToString() != null)
                {

                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                    IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();

                    BrokerSResponseDto BrokerData = BrokerManagementService.GetAllBrokers(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                    return BrokerData.Brokers.ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        [HttpGet]
        [AcceptVerbs("GET", "POST")]
        public object GetBrokerDetailsByBrokerId(JObject data)
        {
            try
            {
                if (Request.Headers.Authorization.ToString() != null)
                {

                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                    IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();

                    BrokerSResponseDto BrokerData = BrokerManagementService.GetBrokerDetailsByBrokerId(
                    SecurityHelper.Context,
                    AuditHelper.Context,
                    Guid.Parse(data["Id"].ToString()));
                    return BrokerData.Brokers.ToArray();
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
        public object GetAllBrokersByCountry(JObject data)
        {
            try
            {
                if (Request.Headers.Authorization.ToString() != null)
                {

                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();
                    BrokerSResponseDto BrokerData = BrokerManagementService.GetAllBrokersByCountry(
                    SecurityHelper.Context,
                    AuditHelper.Context,
                    Guid.Parse(data["CountryId"].ToString()));
                    return BrokerData.Brokers.ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }





        public object UpdateBroker(JObject data) {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            BrokerRequestDto BrokerData = new BrokerRequestDto();
            BrokerData.Id = data["Id"].ToString() == "" ? Guid.Empty : Guid.Parse(data["Id"].ToString());
            BrokerData.Code = data["Code"].ToString();
            BrokerData.Name = data["Name"].ToString();
            BrokerData.BrokerStatus = data["Status"].ToString();
            BrokerData.TelNumber = data["TelNumber"].ToString();
            BrokerData.CountryId = data["CountryId"].ToString() == "" ? Guid.Empty : Guid.Parse(data["CountryId"].ToString());
            BrokerData.Address = data["Address"].ToString();

            IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();
            bool status = BrokerManagementService.UpdateBroker(BrokerData, SecurityHelper.Context, AuditHelper.Context);
            return status;
        }

        [HttpGet]
        public bool IsExsistsBrokerCode(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBrokerManagementService BrokerManagementService = ServiceFactory.GetBrokerManagementService();
            bool status = BrokerManagementService.IsExsistsBrokerCode(
                Guid.Parse(data["Id"].ToString()),                
                data["Name"].ToString(),
                data["Code"].ToString(),
                SecurityHelper.Context, 
                AuditHelper.Context);
            return status; 
        }
    }
}
