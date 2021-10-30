
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
    public class ClaimBordxManegementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region ClaimBordxManegement
        [HttpPost]
        public string CreateClaimBordx(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ClaimBordxRequestDto ClaimBordx = data.ToObject<ClaimBordxRequestDto>();
                IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
                return ClaimBordxManagementService.AddClaimBordx(ClaimBordx, SecurityHelper.Context, AuditHelper.Context);
                //logger.Info("ClaimBordx Added");
                //if (result.ClaimBordxInsertion)
                //{
                //    return "OK";
                //}
                //else
                //{
                //    return "Add ClaimBordx failed!";
                //}
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add ClaimBordx failed!";
            }

        }


        [HttpPost]
        public object GetAllBordxs()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();

            ClaimBordxsResponseDto ClaimBordxsData = ClaimBordxManagementService.GetBordxs(SecurityHelper.Context, AuditHelper.Context);
            return ClaimBordxsData.ClaimBordxs.ToArray();
        }

        [HttpPost]
        public string GetNextBordxNumber(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            int year = Convert.ToInt32(data["year"].ToString());
            int month = Convert.ToInt32(data["month"].ToString());
            Guid insurerId = Guid.Parse(data["insurerId"].ToString());
            Guid ReinsurerId = Guid.Parse(data["ReinsurerId"].ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.GetNextBordxNumber(year, month, insurerId, ReinsurerId, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public string DeleteClaimBordx(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid ClaimBordxId = Guid.Parse(data["bordxId"].ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.DeleteClaimBordx(ClaimBordxId, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public object GetConfirmedClaimBordxYears()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            object ConfirmedClaimBordxYears = ClaimBordxManagementService.ConfirmedClaimBordxYears(SecurityHelper.Context, AuditHelper.Context);
            return ConfirmedClaimBordxYears;
        }

        [HttpPost]
        public object GetConfirmedClaimBordxForGrid(ConfirmedClaimBordxForGridRequestDto ConfirmedClaimBordxForGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            object ConfirmedClaimBordxForGridResponse = ClaimBordxManagementService.GetConfirmedClaimBordxForGrid
             (
             ConfirmedClaimBordxForGridRequestDto,
             SecurityHelper.Context,
             AuditHelper.Context
             );
            return ConfirmedClaimBordxForGridResponse;
        }


        [HttpPost]
        public object GetAllClaimBordxByDateForGrid(GetAllClaimBordxByDateRequestDto GetAllClaimBordxByDateRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();

            object PolicyList = ClaimBordxManagementService.GetAllClaimBordxByDateForGrid(GetAllClaimBordxByDateRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            return PolicyList;
        }

        [HttpPost]
        public string ClaimBordxReopen(ClaimBordxReopenRequestDto ClaimBordxReopenRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.ClaimBordxReopen(ClaimBordxReopenRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetConfirmedClaimBordxYearlybyYear()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            object GetConfirmedClaimBordxYearlybyYear = ClaimBordxManagementService.GetConfirmedClaimBordxYearlybyYear(SecurityHelper.Context, AuditHelper.Context);
            return GetConfirmedClaimBordxYearlybyYear;
        }

        [HttpPost]
        public object GetAllClaimBordxByYearandCountryForGrid(GetAllClaimBordxByYearandCountryRequestDto GetAllClaimBordxByYearandCountryRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();

            object PolicyList = ClaimBordxManagementService.GetAllClaimBordxByYearandCountryForGrid(GetAllClaimBordxByYearandCountryRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            return PolicyList;
        }

        [HttpPost]
        public string ClaimBordxYealyReopen(ClaimBordxYearlyReopenRequestDto ClaimBordxYearlyReopenRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.ClaimBordxYealyReopen(ClaimBordxYearlyReopenRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }

        #endregion

        #region ClaimBordxYearProcess

        [HttpPost]
        public object GetClaimBordxYearsForProcess()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();

            ClaimBordxsYearsResponseDto ClaimBordxsYearsData = ClaimBordxManagementService.GetClaimBordxYearsForProcess(SecurityHelper.Context, AuditHelper.Context);
            return ClaimBordxsYearsData.BordxYears;
        }

        [HttpPost]
        public string ClaimBordxYearProcess(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            int year = Convert.ToInt32(data["BordxYear"].ToString());
            Guid Reinsurer = Guid.Parse(data["Reinsurer"].ToString());
            Guid Insurer = Guid.Parse(data["Insurer"].ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.ClaimBordxYearProcess(year, Reinsurer, Insurer, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public string ClaimBordxYearProcessConfirm(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            int year = Convert.ToInt32(data["BordxYear"].ToString());
            Guid Reinsurer = Guid.Parse(data["Reinsurer"].ToString());
            Guid Insurer = Guid.Parse(data["Insurer"].ToString());

            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            return ClaimBordxManagementService.ClaimBordxYearProcessConfirm(year, Reinsurer, Insurer, SecurityHelper.Context, AuditHelper.Context); ;
        }

        #endregion

        [HttpPost]
        public object GetClaimBordxBordxNumbers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxManagementService ClaimBordxManagementService = ServiceFactory.GetClaimBordxManagementService();
            object GetClaimBordxBordxNumbers = ClaimBordxManagementService.GetClaimBordxBordxNumbers(SecurityHelper.Context, AuditHelper.Context);
            return GetClaimBordxBordxNumbers;
        }

    }

}