using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class IncurredErningManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public object GetUNWYears()
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IIncurredErningManagementService IncurredErningManagementService = ServiceFactory.GetIncurredErningManagementService();
                Response = IncurredErningManagementService.GetUNWYears(SecurityHelper.Context, AuditHelper.Context);
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
        public object GetAllMwMonths(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
                object ManufacturerWarrantyD = new ManufacturerWarrantyResponseDto();

                ManufacturerWarrantyD = ManufacturerWarrantyManagementService.GetManufacturerDetailsByCountryId(Guid.Parse(data["CountryId"].ToString()),
               Guid.Parse(data["ModelId"].ToString()), Guid.Parse(data["MakeId"].ToString()), SecurityHelper.Context,
                   AuditHelper.Context);
                return ManufacturerWarrantyD;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        [HttpPost]
        public object IncurredErningProcess(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            string UNWyear = data["UNWYear"].ToString();
            Guid Reinsurer = Guid.Empty;
            Guid Insurer = Guid.Empty;
            Guid Dealer = Guid.Empty;
            Guid CountryId = Guid.Empty;
            Guid MakeId = Guid.Empty;
            Guid ModelId = Guid.Empty;
            Guid CylindercountId = Guid.Empty;
            Guid EngineCapacityId = Guid.Empty;
            Guid InsuaranceLimitationId = Guid.Empty;
            string BordxStartDate = null;
            string BordxEndDate = null;
            string ErnedDate = null;
            string ClaimedDate = null;
            if (data["UNWYear"].ToString() != null)
            {
                UNWyear = data["UNWYear"].ToString();
            }
            if (data["ReInsurerId"].ToString() != "")
            {
                Reinsurer = Guid.Parse(data["ReInsurerId"].ToString());
            }
            if (data["insurerId"].ToString() != "")
            {
                Insurer = Guid.Parse(data["insurerId"].ToString());
            }
            if (data["DealerId"].ToString() != "")
            {
                Dealer = Guid.Parse(data["DealerId"].ToString());
            }
            if (data["CountryId"].ToString() != "")
            {
                CountryId = Guid.Parse(data["CountryId"].ToString());
            }
            if (data["MakeId"].ToString() != "")
            {
                MakeId = Guid.Parse(data["MakeId"].ToString());
            }
            if (data["ModelId"].ToString() != "")
            {
                ModelId = Guid.Parse(data["ModelId"].ToString());
            }
            if (data["CylindercountId"].ToString() != "")
            {
                CylindercountId = Guid.Parse(data["CylindercountId"].ToString());
            }
            if (data["EngineCapacityId"].ToString() != "")
            {
                EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
            }
            if (data["InsuaranceLimitationId"].ToString() != "")
            {
                InsuaranceLimitationId = Guid.Parse(data["InsuaranceLimitationId"].ToString());
            }
            if (data["BordxStartDate"].ToString() != "")
            {
                BordxStartDate = data["BordxStartDate"].ToString();
            }
            if (data["BordxEndDate"].ToString() != "")
            {
                BordxEndDate = data["BordxEndDate"].ToString();
            }
            if (data["ErnedDate"].ToString() != "")
            {
                ErnedDate = data["ErnedDate"].ToString();
            }
            if (data["ClaimedDate"].ToString() != "")
            {
                ClaimedDate = data["ClaimedDate"].ToString();
            }

            IIncurredErningManagementService IncurredErningManagementService = ServiceFactory.GetIncurredErningManagementService();
            IncurredErningProcessResponseDto IncurredErning = IncurredErningManagementService.IncurredErningProcess(UNWyear,  CountryId,  Dealer, Reinsurer, Insurer, MakeId, ModelId, CylindercountId, EngineCapacityId,BordxStartDate,BordxEndDate, ErnedDate,ClaimedDate, SecurityHelper.Context, AuditHelper.Context);
            if (IncurredErning.IncurredErnings != null)
            { return IncurredErning.IncurredErnings.ToArray(); }
            else
            { return null; }


        }

        [HttpPost]
        public HttpResponseMessage IncurredErningExcel(JObject data)
        {
            HttpResponseMessage result = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string UNWyear = data["UNWYear"].ToString();
                Guid Reinsurer = Guid.Empty;
                Guid Insurer = Guid.Empty;
                Guid Dealer = Guid.Empty;
                Guid CountryId = Guid.Empty;
                Guid MakeId = Guid.Empty;
                Guid ModelId = Guid.Empty;
                Guid CylindercountId = Guid.Empty;
                Guid EngineCapacityId = Guid.Empty;
                Guid InsuaranceLimitationId = Guid.Empty;
                string BordxStartDate = null;
                string BordxEndDate = null;
                string ErnedDate = null;
                string ClaimedDate = null;
                if (data["UNWYear"].ToString() != null)
                {
                     UNWyear = data["UNWYear"].ToString();
                }
                if (data["ReInsurerId"].ToString() != "")
                {
                     Reinsurer = Guid.Parse(data["ReInsurerId"].ToString());
                }
                if (data["insurerId"].ToString() != "")
                {
                     Insurer = Guid.Parse(data["insurerId"].ToString());
                }
                if (data["DealerId"].ToString() != "")
                {
                     Dealer = Guid.Parse(data["DealerId"].ToString());
                }
                if (data["CountryId"].ToString() != "")
                {
                     CountryId = Guid.Parse(data["CountryId"].ToString());
                }
                try {
                    if (data["MakeId"].ToString() != "")
                    {
                        MakeId = Guid.Parse(data["MakeId"].ToString());
                    }
                }
                catch (Exception e) {

                }

                if (data["ModelId"].ToString() != "")
                {
                     ModelId = Guid.Parse(data["ModelId"].ToString());
                }
                if (data["CylindercountId"].ToString() != "")
                {
                     CylindercountId = Guid.Parse(data["CylindercountId"].ToString());
                }
                if (data["EngineCapacityId"].ToString() != "")
                {
                     EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                }
                if (data["InsuaranceLimitationId"].ToString() != "")
                {
                     InsuaranceLimitationId = Guid.Parse(data["InsuaranceLimitationId"].ToString());
                }
                if (data["BordxStartDate"].ToString() != "")
                {
                     BordxStartDate = data["BordxStartDate"].ToString();
                }
                if (data["BordxEndDate"].ToString() != "")
                {
                     BordxEndDate = data["BordxEndDate"].ToString();
                }
                if (data["ErnedDate"].ToString() != "")
                {
                     ErnedDate = data["ErnedDate"].ToString();
                }
                if (data["ClaimedDate"].ToString() != "")
                {
                     ClaimedDate = data["ClaimedDate"].ToString();
                }

                if (ErnedDate ==null || ErnedDate=="")
                {
                    ErnedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
                }

                if (ClaimedDate == null || ClaimedDate == "")
                {
                    ClaimedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
                }

                if (BordxStartDate == null || BordxStartDate == "") {
                    BordxStartDate = DateTime.MinValue.ToString();
                }

                if (BordxEndDate == null || BordxEndDate == "")
                {
                    BordxEndDate = DateTime.MinValue.ToString();
                }

                IIncurredErningManagementService IncurredErningManagementService = ServiceFactory.GetIncurredErningManagementService();
                IncurredErningExportResponseDto Response = IncurredErningManagementService.GetIncurredErningExcel(UNWyear, CountryId, Dealer, Reinsurer, Insurer, MakeId, ModelId, CylindercountId, EngineCapacityId, InsuaranceLimitationId, BordxStartDate, BordxEndDate, ErnedDate, ClaimedDate, SecurityHelper.Context, AuditHelper.Context);

                var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenarateIncurdErningExcel(Response, ErnedDate);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Lost Ratio" + UNWyear + "_" + ".xlsx"
                };
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;


        }

    }
}
