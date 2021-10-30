using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{

    public class ClaimInvoiceController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public object AddClaimInvoice(JObject data)
        {
            try
            {
                ClaimInvoiceEntryRequestDto claimInvoiceEntry = data.ToObject<ClaimInvoiceEntryRequestDto>();
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                ClaimInvoiceEntryRequestDto result = claimInvoiceManagementService.AddClaimInvoice(claimInvoiceEntry, SecurityHelper.Context, AuditHelper.Context);
                Logger.Info("Claim Invoice Added");
                if (result.ClaimInvoiceEntryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Claim Invoice failed!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Claim Invoice failed!";
            }

        }

        [HttpPost]
        public object AddAjusments(JObject data)
        {
            object Response = null;
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ClaimId = Guid.Parse(data["ClaimId"].ToString());
                decimal AdjustPartAmount = Convert.ToDecimal(data["adjustPartAmount"].ToString());
                decimal AdjustLabourAmount = Convert.ToDecimal(data["adjustLabourAmount"].ToString());
                decimal AdjustSundryAmount = Convert.ToDecimal(data["adjustSundryAmount"].ToString());
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                Response = claimInvoiceManagementService.AddAjusments(ClaimId, AdjustPartAmount, AdjustLabourAmount, AdjustSundryAmount, SecurityHelper.Context, AuditHelper.Context);

                return Response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Claim Invoice failed!";
            }

        }

        [HttpPost]
        //[Route("RetriveInvoiceEntryDataForDashboard")]
        public object RetriveInvoiceEntryDataForDashboard(JObject data)
        {
            object response = null;
            try
            {
                var ClaimSubmittedDealerId = Guid.Parse(data["ClaimSubmittedDealerId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                response = claimInvoiceManagementService.RetriveInvoiceEntryData(ClaimSubmittedDealerId,
                    SecurityHelper.Context, AuditHelper.Context);
                return response;
            }
            catch (Exception ex)
            {
                response = "Error occured";
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllClaimByDealerId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid claimSubmittedDealerId = Guid.Parse(data["ClaimSubmittedDealerId"].ToString());
                string claimNumber = data["ClaimNumber"].ToString();
                string invoiceNumber = data["InvoiceNumber"].ToString();
                if (String.IsNullOrEmpty(invoiceNumber)) {
                    IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                    Response = claimInvoiceManagementService.GetAllClaimByDealerId(claimSubmittedDealerId, claimNumber,
                        SecurityHelper.Context, AuditHelper.Context);
                }

                return Response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllSubmittedInvoiceClaimByDealerId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid claimSubmittedDealerId = Guid.Parse(data["ClaimSubmittedDealerId"].ToString());
                string invoiceNumber = data["InvoiceNumber"].ToString();
                string claimNumber = data["ClaimNumber"].ToString();

                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                Response = claimInvoiceManagementService.GetAllSubmittedInvoiceClaimByDealerId(claimSubmittedDealerId, invoiceNumber, claimNumber,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllClaimPartDetailsByClaimId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ClaimId = Guid.Parse(data["ClaimId"].ToString());
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                Response = claimInvoiceManagementService.GetAllClaimPartDetailsByClaimId(ClaimId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllClaimInvoiceEntryForSearchGrid(ClaimInvoiceEntrySearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();

            return claimInvoiceManagementService.GetAllClaimInvoiceEntryForSearchGrid(
            ClaimInvoiceEntrySearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllClaimInvoiceEntryClaimForSearchGrid(ClaimInvoiceEntryClaimSearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();

            return claimInvoiceManagementService.GetAllClaimInvoiceEntryClaimForSearchGrid(
            ClaimInvoiceEntrySearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetClaimInvoiceEntryById(JObject data)
        {

            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ClaimInvoiceEntryId = Guid.Parse(data["Id"].ToString());
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                Response = claimInvoiceManagementService.GetClaimInvoiceEntryById(ClaimInvoiceEntryId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public string UpdateClaimInvoice(JObject data)
        {
            string Response = string.Empty;
            try
            {
                ClaimInvoiceEntryRequestDto ClaimInvoice = data.ToObject<ClaimInvoiceEntryRequestDto>();
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                ClaimInvoiceEntryRequestDto result = claimInvoiceManagementService.UpdateClaimInvoice(ClaimInvoice, SecurityHelper.Context, AuditHelper.Context);

                Logger.Info("Claim Invoice Added");
                if (result.ClaimInvoiceEntryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Claim Invoice failed!";
                }

            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while updating Claim Invoice";
            }
            return Response;
        }

        [HttpPost]
        public string ConfirmClaimInvoice(JObject data)
        {
            string Response = string.Empty;
            try
            {
                ClaimInvoiceEntryRequestDto ClaimInvoice = data.ToObject<ClaimInvoiceEntryRequestDto>();
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                ClaimInvoiceEntryRequestDto result = claimInvoiceManagementService.ConfirmClaimInvoice(ClaimInvoice, SecurityHelper.Context, AuditHelper.Context);

                Logger.Info("Confirm Claim Invoice Added");
                if (result.ClaimInvoiceEntryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Claim Invoice failed!";
                }

            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while updating Claim Invoice";
            }
            return Response;
        }

    }
}