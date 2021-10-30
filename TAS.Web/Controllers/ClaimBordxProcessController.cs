using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Notification;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    [RoutePrefix("api/claimbordxprocess")]
    public class ClaimBordxProcessController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Validate logged in user can have access on this page
        /// </summary>
        /// <returns>int list</returns>
        [HttpPost]
        [Route("GetClaimBordxYears")]
        public object GetClaimBordxYears()
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.GetClaimBordxYears(SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetBordxYearsByClaim")]
        public object GetBordxYearsByClaim()
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.GetBordxYearsByClaim(SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        /// <summary>
        /// GetClaimBordx By Year And Month
        /// </summary>
        /// <param name="data">year & month</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetClaimBordxByYearAndMonth")]
        public object GetClaimBordxByYearAndMonth(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                int year = Convert.ToInt32(data["year"].ToString());
                int month = Convert.ToInt32(data["month"].ToString());
                Guid Insurerid = Guid.Parse(data["insurerid"].ToString());
                Guid Reinsurerid = Guid.Parse(data["reinsurerid"].ToString());

                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.GetClaimBordxByYearAndMonth(year, month, Insurerid, Reinsurerid, SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
        [HttpPost]
        [Route("ProcessClaimBordxForSearchGrid")]
        public object ProcessClaimBordxForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();

            return claimBordxProcessManagementService.ProcessClaimBordxForSearchGrid(
                ClaimSearchGridRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
        }
        /// <summary>
        /// Add and Process ClaimBordxProcess
        /// </summary>
        /// <param name="data">ClaimBordxProcessRequestDto</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClaimBordxProcess")]
        public object ClaimBordxProcess(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid claimbordxId = Guid.Parse(data["claimbordxId"].ToString());
                Guid userId = Guid.Parse(data["userId"].ToString());
                bool isProcess = Convert.ToBoolean(data["isProcess"].ToString());
                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.ClaimBordxProcess(claimbordxId, userId, isProcess, SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        /// <summary>
        /// Add and Process ClaimBordxProcess
        /// </summary>
        /// <param name="data">ClaimBordxProcessRequestDto</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClaimBordxProcessUpdate")]
        public object ClaimBordxProcessUpdate(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid claimbordxId = Guid.Parse(data["claimbordxId"].ToString());
                Guid userId = Guid.Parse(data["userId"].ToString());
                bool isConfirm = Convert.ToBoolean(data["isConfirm"].ToString());
                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.ClaimBordxProcessUpdate(claimbordxId, userId, isConfirm, SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        /// <summary>
        /// Get Processed ClaimBordx details By Year
        /// </summary>
        /// <param name="data">year param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClaimBordxProcessedDetailsByYear")]
        public object ClaimBordxProcessedDetailsByYear(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                int year = Convert.ToInt32(data["year"].ToString());
                Guid Insurerid = Guid.Parse(data["insurerid"].ToString());
                Guid Reinsurerid = Guid.Parse(data["reinsurerid"].ToString());
                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();

                Response = claimBordxProcessManagementService.ClaimBordxProcessedDetailsByYear(year, Insurerid, Reinsurerid, SecurityHelper.Context, AuditHelper.Context);
                return Response;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        /// <summary>
        /// ClaimBordx can confirmed By Year
        /// </summary>
        /// <param name="data">year</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClaimBordxCanConfirmedByYear")]
        public object ClaimBordxCanConfirmedByYear(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                int year = Convert.ToInt32(data["year"].ToString());
                Guid insurerid = Guid.Parse(data["insurerid"].ToString());
                Guid reinsurerid = Guid.Parse(data["reinsurerid"].ToString());

                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                Response = claimBordxProcessManagementService.ClaimBordxCanConfirmedByYear(year, insurerid, reinsurerid, SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
        /// <summary>
        /// ExportToExcelClaimBordxById
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportToExcelClaimBordxById")]
        public HttpResponseMessage ExportToExcelClaimBordxById(JObject data)
        {
            HttpResponseMessage result = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid claimbordxid = Guid.Parse(data["claimbordxid"].ToString());

                IClaimBordxProcessManagementService claimBordxProcessManagementService = ServiceFactory.GetClaimBordxProcessManagementService();
                ClaimBordxExportResponseDto Response = claimBordxProcessManagementService.GetProcessClaimBordxExport(claimbordxid, SecurityHelper.Context, AuditHelper.Context);

                var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenarateClaimBordxExcel(Response);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Response.ClaimBordxHeader.FileName + ".xlsx" //"Bordx_" + "2017" + "_" +"Month" + "_" + "test" + ".xlsx"
                };
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;
        }

        //[HttpPost]
        //public HttpResponseMessage ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        //{
        //    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
        //    IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

        //    BordxExportResponseDto BordxResponse = PolicyManagementService.ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto,
        //        SecurityHelper.Context,
        //        AuditHelper.Context);

        //    var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenerateBordxExcel(BordxResponse);

        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    result.Content = new StreamContent(stream);
        //    result.Content.Headers.ContentType =
        //        new MediaTypeHeaderValue("application/octet-stream");
        //    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = "Bordx_" + BordxResponse.BordxYear + "_" +
        //        BordxResponse.BordxMonth + "_" + BordxResponse.TpaName + ".xlsx"
        //    };
        //    return result;
        //}

    }
}
