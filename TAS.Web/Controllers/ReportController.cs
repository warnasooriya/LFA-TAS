using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        [Route("GetAllReportInformationByUserId")]
        public object GetAllReportInformationByUserId(JObject data)
        {
            object Response = null;
            try
            {

                Guid loggedInUserId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["loggedInUserId"].ToString(), out loggedInUserId);
                if (!guidParseRes)
                {
                    loggedInUserId = JsonConvert.DeserializeObject<Guid>(data["loggedInUserId"].ToString());
                }
                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());
                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.GetAllReportInformationByUserId(loggedInUserId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetAllReportParamInformationByReportId")]
        public object GetAllReportParamInformationByReportId(JObject data)
        {
            object Response = null;
            try
            {
                Guid reportId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["reportId"].ToString(), out reportId);
                if (!guidParseRes)
                {
                    reportId = JsonConvert.DeserializeObject<Guid>(data["reportId"].ToString());
                }

                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());
                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.GetAllReportParamInformationByReportId(reportId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetAllDataForReportDropdown")]
        public object GetAllDataForReportDropdown(JObject data)
        {
            object Response = null;
            try
            {
                Guid reportParamId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["paramId"].ToString(), out reportParamId);
                if (!guidParseRes)
                {
                    reportParamId = JsonConvert.DeserializeObject<Guid>(data["reportId"].ToString());
                }

                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());
                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.GetAllDataForReportDropdownElement(reportParamId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        [Route("GetAllDataForReportFromParentDropdown")]
        public object GetAllDataForReportFromParentDropdown(JObject data)
        {
            object Response = null;
            try
            {
                Guid reportParamId, reportParamParentValue, parentParamId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["paramId"].ToString(), out reportParamId);
                bool guidParseResData = Guid.TryParse(data["parentParamValue"].ToString(), out reportParamParentValue);
                bool guidParseParentId = Guid.TryParse(data["parentParamId"].ToString(), out parentParamId);

                if (!guidParseRes)
                {
                    reportParamId = JsonConvert.DeserializeObject<Guid>(data["reportId"].ToString());
                }

                if (!guidParseResData)
                {
                    reportParamParentValue = JsonConvert.DeserializeObject<Guid>(data["parentParamValue"].ToString());
                }

                if (!guidParseParentId)
                {
                    parentParamId = JsonConvert.DeserializeObject<Guid>(data["parentParamId"].ToString());
                }

                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());
                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.GetAllDataForReportFromParentDropdown(reportParamId,
                    reportParamParentValue, parentParamId,SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        [Route("ViewReport")]
        public object ViewReport(JObject data)
        {
            object Response = null;
            try
            {
                Guid reportId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["reportId"].ToString(), out reportId);
                if (!guidParseRes)
                    reportId = JsonConvert.DeserializeObject<Guid>(data["reportId"].ToString());
                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());

                List<ReportParameterDataRequestDto> paramList = JsonConvert.DeserializeObject<List<ReportParameterDataRequestDto>>(data["params"].ToString());

                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.ViewReport(reportId, paramList,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }
        [HttpPost]
        public HttpResponseMessage ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            BordxExportResponseDto BordxResponse = PolicyManagementService.ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenerateBordxExcel(BordxResponse);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Bordx_" + BordxResponse.BordxYear + "_" +
                BordxResponse.BordxMonth + "_" + BordxResponse.TpaName + ".xlsx"
            };
            return result;
        }


        [HttpPost]
        [Route("excelReport")]
        public object excelReport(JObject data)
        {
            object Response = null;
            try
            {
                Guid reportId = Guid.Empty;
                bool guidParseRes = Guid.TryParse(data["reportId"].ToString(), out reportId);
                if (!guidParseRes)
                    reportId = JsonConvert.DeserializeObject<Guid>(data["reportId"].ToString());
                var authorizationKey = JsonConvert.DeserializeObject<string>(data["Authorization"].ToString());

                List<ReportParameterDataRequestDto> paramList = JsonConvert.DeserializeObject<List<ReportParameterDataRequestDto>>(data["params"].ToString());

                SecurityHelper.Context.setToken(authorizationKey);
                var reportManagementService = ServiceFactory.GetReportManagementService();
                Response = reportManagementService.ExcelReport(reportId, paramList,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

    }
}
