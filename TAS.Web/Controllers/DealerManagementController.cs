
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DealerManagementController : ApiController
    {
        //private static Logger nlogger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public string AddDealer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                DealerRequestDto Dealer = data.ToObject<DealerRequestDto>();
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                var result = DealerManagementService.AddDealer(Dealer, SecurityHelper.Context, AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured while saving dealer.";
            }

        }

        [HttpPost]
        public string UpdateDealer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                DealerRequestDto Dealer = data.ToObject<DealerRequestDto>();
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                var result = DealerManagementService.UpdateDealer(Dealer, SecurityHelper.Context, AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured while updating dealer.";
            }

        }

        [HttpPost]
        public object GetDealerById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                DealerRespondDto Dealer = DealerManagementService.GetDealerById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return Dealer;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
        [HttpPost]
        public object GetAllDealersByUserId(JObject data)
        {
            try
            {
                Guid UserId = Guid.Parse(data["Id"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                DealersRespondDto Dealer = DealerManagementService.GetAllDealersByUserId(UserId,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return Dealer;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetDealerStaffByDealerIdandBranchId(JObject data)
        {
            try
            {
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                Guid BranchId = Guid.Parse(data["BranchId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                object Dealer = DealerManagementService.GetDealerStaffByDealerIdandBranchId(DealerId, BranchId,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return Dealer;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetDealerStaffByDealerId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                List<DealerStaffResponseDto> Staffs = DealerManagementService.GetDealerStaffs(
                    SecurityHelper.Context,
                    AuditHelper.Context).DealerStaffs;
                if (Staffs != null && Staffs.Count != 0)
                {
                    Staffs = Staffs.FindAll(d => d.DealerId == Guid.Parse(data["Id"].ToString()));
                }
                return Staffs;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetYearsMonthsForDealerInvoices()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                return DealerManagementService.GetYearsMonthsForDealerInvoices(
                    SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object AddDealerStaff(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                List<DealerStaffRequestDto> Staff = data["Staff"].ToObject<List<DealerStaffRequestDto>>();
                List<DealerBranchRequestDto> DealerBranch = data["DealerBranch"].ToObject<List<DealerBranchRequestDto>>();
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                List<DealerStaffResponseDto> allStaff = DealerManagementService.GetDealerStaffs(SecurityHelper.Context, AuditHelper.Context).DealerStaffs.FindAll(s => s.DealerId == Staff.FirstOrDefault().DealerId);
                DealerStaffAddResponse result = DealerManagementService.AddDealerStaff(Staff, DealerBranch, SecurityHelper.Context, AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return String.Empty;
            }


        }

        [HttpPost]
        public string AddDealerComment(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                AddDealerCommentRequestDto dealerComment = data.ToObject<AddDealerCommentRequestDto>();
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                var result = DealerManagementService.AddDealerComment(dealerComment, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Dealer Comment  Added");

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return String.Empty;
            }

        }


        [HttpPost]
        public object GetAllDealers()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                DealersRespondDto DealerData = DealerManagementService.GetAllDealers(
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerData.Dealers.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetBordxDealers(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

                List<PolicyBundleResponseDto> policies = PolicyManagementService.GetPolicyBundles(
                SecurityHelper.Context,
                AuditHelper.Context).Policies.FindAll(p => p.IsApproved.Equals(true)
                    //&& p.PolicySoldDate.Year.ToString() == data["Year"].ToString()
                    //&& p.PolicySoldDate.Month.ToString() == data["Month"].ToString()
                    //&& p.CommodityTypeId == Guid.Parse(data["CommodityType"].ToString())
                    );

                var DealerList = policies.Select(d => d.DealerId);

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();

                List<DealerRespondDto> DealerData = DealerManagementService.GetAllDealers(
                SecurityHelper.Context,
                AuditHelper.Context).Dealers.FindAll(d => DealerList.Contains(d.Id));

                return DealerData.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        [HttpPost]
        public object GetDealersByBordx(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid bordxId = Guid.Parse(data["bordxId"].ToString());
                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                response = DealerManagementService.GetDealersByBordx(bordxId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;

        }

        [HttpPost]
        public object GetAllDealersByCountryId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                Guid countryId = Guid.Parse(data["Id"].ToString());
                DealersRespondDto DealerData = DealerManagementService.GetAllDealersByCountry(
                    countryId,
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerData.Dealers.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        [HttpPost]
        public string AddDealerLocation(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                DealerLocationRequestDto DealerLocation = data.ToObject<DealerLocationRequestDto>();
                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                DealerLocationRequestDto result = DealerLocationManagementService.AddDealerLocation(DealerLocation, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("DealerLocation  Added");
                if (result.DealerLocationInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add DealerLocation  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add DealerLocation  failed!";
            }

        }

        [HttpPost]
        public string UpdateDealerLocation(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                DealerLocationRequestDto DealerLocation = data.ToObject<DealerLocationRequestDto>();
                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                DealerLocationRequestDto result = DealerLocationManagementService.UpdateDealerLocation(DealerLocation, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("DealerLocation  Added");
                if (result.DealerLocationInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add DealerLocation  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add DealerLocation  failed!";
            }

        }

        [HttpPost]
        public object GetDealerContrctById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();

                DealerLocationRespondDto DealerLocation = DealerLocationManagementService.GetDealerLocationById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return DealerLocation;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GenerateDealerInvoices(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerInvoicesGenerateRequestDto DealerInvoicesGenerateRequest = data.ToObject<DealerInvoicesGenerateRequestDto>();
                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();

                DealerInvoicesGenerateResponseDto Response = DealerLocationManagementService.GenerateDealerInvoices(
                    DealerInvoicesGenerateRequest,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                String documentName = String.Empty;
                var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenerateDealerInvoiceExcel(Response, out documentName);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = documentName + ".xlsx"
                };
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.NoContent);

            }

        }

        [HttpPost]
        public object GetAllDealerLocations()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                DealerLocationsRespondDto DealerLocationData = DealerLocationManagementService.GetAllDealerLocations(
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerLocationData.DealerLocations.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetAllDealerLocationById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                DealerLocationsRespondDto DealerLocationData = DealerLocationManagementService.GetAllDealerLocations(
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerLocationData.DealerLocations.Find(d => d.Id == Guid.Parse(data["Id"].ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetAllDealerLocationsByDealerId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                DealerLocationsRespondDto DealerLocationData = DealerLocationManagementService.GetAllDealerLocations(
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerLocationData.DealerLocations.Where(m => m.DealerId == Guid.Parse(data["Id"].ToString())).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetAllDealerLocationsByUserId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                object DealerLocationData = DealerLocationManagementService.GetAllDealerLocationsByUser(Guid.Parse(data["userId"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealerLocationData;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetAllDealerStaffLocationsByDealerId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();

                object DealerLocationData = DealerLocationManagementService.GetAllDealerStaffLocationsByDealerId(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return DealerLocationData;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        #region Other
        [HttpPost]
        public object GetAllCommodities()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();

            CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(
            SecurityHelper.Context,
            AuditHelper.Context);
            if (CommoditiesData == null)
            {
                return null;
            }

            return CommoditiesData.Commmodities.ToArray();
        }

        [HttpPost]
        public object GetCurrencies()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyData.Currencies.ToArray();
        }

        [HttpPost]
        public object GetCurrencyById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyData.Currencies.Find(c => c.Id == Guid.Parse(data["Id"].ToString()));
        }
        #endregion


        [HttpPost]
        public object GetAllDiscountSchemes(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllDiscountSchemes(
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object SearchDealerDiscountSchemes(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerDiscountSchemesSearchRequestDto dealerDiscountSchemesSearchRequest = data.ToObject<DealerDiscountSchemesSearchRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.SearchDealerDiscountSchemes(dealerDiscountSchemesSearchRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object SaveDealerDiscount(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerDiscountSaveRequestDto dealerDiscountSaveRequest = data.ToObject<DealerDiscountSaveRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.SaveDealerDiscount(dealerDiscountSaveRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object GetDealerDiscountById(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid dealerDiscountId = Guid.Parse(data["dealerDiscountId"].ToString());
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetDealerDiscountById(dealerDiscountId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object AddDealerLabourCharge(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerLabourChargeSaveRequestDto dealerLabourChargeSaveRequest = data.ToObject<DealerLabourChargeSaveRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.AddDealerLabourCharge(dealerLabourChargeSaveRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object SearchDealerLabourChargeSchemes(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerLabourChargeSearchRequestDto dealerLabourChargeSchemesSearchRequest = data.ToObject<DealerLabourChargeSearchRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.SearchDealerLabourChargeSchemes(dealerLabourChargeSchemesSearchRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetDealerLabourChargeById(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid dealerLabourChargeId = Guid.Parse(data["dealerLabourChargeId"].ToString());
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetDealerLabourChargeById(dealerLabourChargeId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object GetAllMakesByDealerId(JObject data)
        {

            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                MakesResponseDto result = dealerLocationManagementService.GetAllMakesByDealerId(dealerId,
                SecurityHelper.Context,
                AuditHelper.Context);
                return result.Makes.ToArray();
            }
            catch (Exception ex)
            {

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetConfirmedBordxByYearAndMonth(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                int year = int.Parse(data["year"].ToString());
                int month = int.Parse(data["month"].ToString());

                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                var result = dealerLocationManagementService.GetConfirmedBordxByYearAndMonth(year, month,
                SecurityHelper.Context,
                AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        [HttpPost]
        public object DownloadInvoiceSummary(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());
                Guid bordxId = Guid.Parse(data["bordxId"].ToString());

                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                var result = dealerLocationManagementService.DownloadInvoiceSummary(dealerId, bordxId,
                SecurityHelper.Context,
                AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        [HttpPost]
        public object GetTyreDetailsByArticleNo(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var articleNo = data["articleNo"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetTyreDetailsByArticleNo(articleNo,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;

        }

        [HttpPost]
        public object GetArticleNoByTyreSize(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var width = data["width"].ToString();
                var cross = data["cross"].ToString();
                var diameter = data["diameter"].ToString();
                var loadSpeed = data["loadSpeed"].ToString();
                var pattern = data["pattern"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetArticleNoByTyreSize(width, cross, diameter, loadSpeed, pattern,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;

        }


        #region dealer invoice code generation
        [HttpPost]
        public object UserValidationDealerInvoiceCode(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid userId = Guid.Parse(data["loggedInUserId"].ToString());
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.ValidateUserOnDealerInvoiceCodeGeneration(userId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GenerateInvoiceCode(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                GenerateInvoiceCodeRequestDto generateInvoiceCodeRequest = data.ToObject<GenerateInvoiceCodeRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GenerateInvoiceCode(generateInvoiceCodeRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object SaveTyrePolicyDetails(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                SaveTyrePolicySalesRequestDto generateInvoiceCodeRequest = data.ToObject<SaveTyrePolicySalesRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.SaveTyrePolicyDetails(generateInvoiceCodeRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object GetTyreContractDetails(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                TyreContractRequestDto tyreContractRequestDto = data.ToObject<TyreContractRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetTyreContractDetails(tyreContractRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object SearchDealerInvoiceCode(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                DealerInvoiceCodeSearchRequestDto invoiceCodeSearchRequest = data.ToObject<DealerInvoiceCodeSearchRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.SearchDealerInvoiceCode(invoiceCodeSearchRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object LoadInvoceCodeDetailsById(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                LoadInvoceCodeByIdRequestDto invoiceLoadByIdRequest = data.ToObject<LoadInvoceCodeByIdRequestDto>();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.LoadInvoceCodeDetailsById(invoiceLoadByIdRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAvailabelTireSizesByloadSpeed(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                //LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto = data.ToObject<LoadTyreDetailsByWidthRequestDto>();
                var Cross = data["frontcross"].ToString();
                var Width = data["frontwidth"].ToString();
                var Diameter = data["frontdiameter"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllAvailabelTireSizesByloadSpeed(Cross, Width, Diameter,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAvailabelTireSizesByPattern(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                //LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto = data.ToObject<LoadTyreDetailsByWidthRequestDto>();
                var Cross = data["frontcross"].ToString();
                var Width = data["frontwidth"].ToString();
                var Diameter = data["frontdiameter"].ToString();
                var LoadSpeed = data["frontloadSpeed"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllAvailabelTireSizesByPattern(Cross, Width, Diameter, LoadSpeed,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAvailabelTireSizesByDiameter(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                //LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto = data.ToObject<LoadTyreDetailsByWidthRequestDto>();
                var Cross = data["frontcross"].ToString();
                var Width = data["frontwidth"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllAvailabelTireSizesByDiameter(Cross, Width,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAvailabelTireSizesByWidth(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto = data.ToObject<LoadTyreDetailsByWidthRequestDto>();
                //var WidthFront = data["frontwidth"].ToString();
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllAvailabelTireSizesByWidth(LoadTyreDetailsByWidthRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAvailabelTireSizes(JObject data)
        {
            object response = new object();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IDealerLocationManagementService dealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
                response = dealerLocationManagementService.GetAllAvailabelTireSizes(SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        #endregion


    }
}
