
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
    public class FaultManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region FaultCategory
        [HttpPost]
        public object GetAllFaultCategory(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultCategorysResponseDto FaultsData = FaultCategoryService.GetAllCatFaults(
            SecurityHelper.Context,
            AuditHelper.Context);
            return FaultsData.Faults.ToArray();
        }

        [HttpPost]
        public string AddFaultCategory(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                FaultCategoryRequestDto FaultCat = data.ToObject<FaultCategoryRequestDto>();
                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultCategoryRequestDto result = FaultCatManagementService.AddFaultCategory(FaultCat, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FaultCategory Added");
                if (result.FaultCategoryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add FaultCategory failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add FaultCategory failed!";
            }

        }

        [HttpPost]
        public string UpdateFaultCategory(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                FaultCategoryRequestDto FaultCat = data.ToObject<FaultCategoryRequestDto>();
                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultCategoryRequestDto result = FaultCatManagementService.UpdateFaultCategory(FaultCat, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FaultCategory Added");
                if (result.FaultCategoryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add FaultCategory failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add FaultCategory failed!";
            }

        }

        [HttpPost]
        public object GetFaultCategoryById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultCategoryResponseDto FaultCat = FaultCategoryService.GetFaultCategoryById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);

            return FaultCat;
        }

        #endregion

        #region FaultArea
        [HttpPost]
        public object GetAllFaultArea(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultAreasResponseDto FaultsData = FaultCategoryService.GetAllFaultAreas(
            SecurityHelper.Context,
            AuditHelper.Context);
            return FaultsData.Faults.ToArray();
        }

        [HttpPost]
        public object GetFaultAreaById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultAreaResponseDto FaultArea = FaultCategoryService.GetFaultAreaById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);

            return FaultArea;
        }

        [HttpPost]
        public string AddFaultArea(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                FaultAreaRequestDto FaultArea = data.ToObject<FaultAreaRequestDto>();
                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultAreaRequestDto result = FaultCatManagementService.AddFaultArea(FaultArea, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FaultArea Added");
                if (result.FaultAreaInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add FaultArea failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add FaultArea failed!";
            }

        }

        [HttpPost]
        public string UpdateFaultArea(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                FaultAreaRequestDto FaultArea = data.ToObject<FaultAreaRequestDto>();
                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultAreaRequestDto result = FaultCatManagementService.UpdateFaultArea(FaultArea, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FaultArea Added");
                if (result.FaultAreaInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add FaultArea failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add FaultArea failed!";
            }

        }
        #endregion

        #region Fault
        [HttpPost]
        public object GetAllFault(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultsResponseDto FaultsData = FaultCategoryService.GetAllFaults(
            SecurityHelper.Context,
            AuditHelper.Context);
            return FaultsData.Faults.ToArray();
        }

        [HttpPost]
        public object GetFaultById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultResponseDto Fault_ = FaultCategoryService.GetFaultById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);

            return Fault_;
        }

        [HttpPost]
        public string AddFault(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                string[] CFailures = data["CFailures"].ToObject<string[]>();
                FaultRequestDto Fault = data["Fault"].ToObject<FaultRequestDto>();
                Fault.CFailures = CFailures;

                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultRequestDto result = FaultCatManagementService.AddFault(Fault, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Fault Added");
                if (result.FaultInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Fault failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Fault failed!";
            }

        }

        [HttpPost]
        public string UpdateFault(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                string[] CFailures = data["CFailures"].ToObject<string[]>();
                FaultRequestDto Fault = data["Fault"].ToObject<FaultRequestDto>();
                Fault.CFailures = CFailures;
                IFaultManagementService FaultCatManagementService = ServiceFactory.GetFaultManagementService();
                FaultRequestDto result = FaultCatManagementService.UpdateFault(Fault, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Fault Added");
                if (result.FaultInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Fault failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Fault failed!";
            }


        }

        [HttpPost]
        public object GetAllFaultCauseOfFailure(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            FaultCauseOfFailuresDto FaultsData = FaultCategoryService.GetAllFaultCauseOfFailures(
            SecurityHelper.Context,
            AuditHelper.Context);
            return FaultsData.FaultCauseOfFailures.ToArray();
        }

        [HttpPost]
        public object GetAllCauseOfFailuresByFaultId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid faultId = Guid.Parse(data["Id"].ToString());
            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            return FaultCategoryService.GetAllCauseOfFailuresByFaultId(faultId,
            SecurityHelper.Context,
            AuditHelper.Context);

        }

        public object SearchFaultsByCriterias(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            FaultSearchRequestDto faultSearchRequestDto = data.ToObject<FaultSearchRequestDto>();
            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();

            return FaultCategoryService.SearchFaultsByCriterias(faultSearchRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllFaultForSearchGrid(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            FaultSearchRequestDto faultSearchRequestDto = data.ToObject<FaultSearchRequestDto>();
            IFaultManagementService FaultCategoryService = ServiceFactory.GetFaultManagementService();
            return FaultCategoryService.GetAllFaultForSearchGrid(
                faultSearchRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object ValidateFaultCode(JObject data)
        {
            object Response = null;

            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string faultCode = data["FaultCode"].ToString();
                Guid faultCategoryId = Guid.Parse(data["faultCategoryId"].ToString());
                Guid faultAreaId = Guid.Parse(data["faultAreaId"].ToString());
                IFaultManagementService faultCategoryService = ServiceFactory.GetFaultManagementService();
                Response = faultCategoryService.ValidateFaultCode(faultCode, faultCategoryId, faultAreaId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch(Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        #endregion
    }
}
