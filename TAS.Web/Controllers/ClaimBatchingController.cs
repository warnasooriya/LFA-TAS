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

    public class ClaimBatchingController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public object AddClaimBatching(JObject data)
        {
            string response = string.Empty;
            try
            {
                ClaimBatchingRequestDto ClaimBatch = data.ToObject<ClaimBatchingRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();

                response = claimBatchingManagementService.AddClaimBatching(ClaimBatch, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while adding new batch";
            }
            return response;
        }


        [HttpPost]
        public object GetNextBatchNumber(JObject data)
        {
            string response = string.Empty;
            try
            {
                ClaimBatchNumberRequestDto claimBatchNumberDetails = data.ToObject<ClaimBatchNumberRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                return claimBatchingManagementService.GetNextBatchNumber(claimBatchNumberDetails, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while getting batch details.";
            }
            return response;
        }

        [HttpPost]
        public object GetLast10BatchesBySearchCritera(JObject data)
        {
            object response = string.Empty;
            try
            {
                ClaimBatchNumberRequestDto claimBatchNumberDetails = data.ToObject<ClaimBatchNumberRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                return claimBatchingManagementService.GetClaimBatchDetails(claimBatchNumberDetails, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while getting batch details.";
            }
            return response;
        }

        [HttpPost]
        public object GetClaimBatchList(JObject data)
        {
            object response = string.Empty;
            try
            {
                ClaimBatchNumberRequestDto claimBatchNumberDetails = data.ToObject<ClaimBatchNumberRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                return claimBatchingManagementService.GetClaimBatchList(claimBatchNumberDetails, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while getting batch details.";
            }
            return response;
        }


        [HttpPost]
        public object GetClaimGroupsByBatchId(JObject data)
        {
            object response = string.Empty;
            try
            {
                Guid claimBatchId = Guid.Parse(data["BatchId"].ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                response = claimBatchingManagementService.GetClaimGroupsByBatchId(claimBatchId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while getting batch details.";
            }
            return response;
        }

        [HttpPost]
        public object GetNextClaimGroupNumberById(JObject data)
        {
            object response = string.Empty;
            try
            {
                Guid claimBatchId = Guid.Parse(data["BatchId"].ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                response = claimBatchingManagementService.GetNextClaimGroupNumberById(claimBatchId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllEligibleClaimsByBatchId(JObject data)
        {
            object response = string.Empty;
            try
            {
                Guid claimBatchId = Guid.Parse(data["BatchId"].ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                response = claimBatchingManagementService.GetAllEligibleClaimsByBatchId(claimBatchId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object GetAllAllocatedClaimsByGroupId(JObject data)
        {
            object response = string.Empty;
            try
            {
                Guid claimBatchId = Guid.Parse(data["BatchId"].ToString());
                Guid claimBatchGroupId = Guid.Parse(data["GroupId"].ToString());

                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                response = claimBatchingManagementService.GetAllAllocatedClaimsByGroupId(claimBatchId, claimBatchGroupId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object SaveClaimBatchGroup(JObject data)
        {
            object response = string.Empty;
            try
            {
                ClaimBatchGroupSaveRequestDto claimBatchSaveRequest = data.ToObject<ClaimBatchGroupSaveRequestDto>();

                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                response = claimBatchingManagementService.SaveClaimBatchGroup(claimBatchSaveRequest, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }



        [HttpPost]
        public object GetAllClaimBatching()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimBatchResponseDto ClaimBatchingData = claimBatchingManagementService.GetAllClaimBatching(SecurityHelper.Context,
                AuditHelper.Context);

                return ClaimBatchingData.ClaimBatchTable.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        public object GetAllClaimDetailsIsBachingFalse()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimsResponseDto ClaimData = claimBatchingManagementService.GetAllClaimDetailsIsBachingFalse(SecurityHelper.Context,
                AuditHelper.Context);

                return ClaimData.Claims.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        public object GetAllClaimBatchingForSearchGrid(ClaimBatchingSearchGridRequestDto ClaimBatchingSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();

            return claimBatchingManagementService.GetAllClaimBatchingForSearchGrid(
            ClaimBatchingSearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllClaimGroupForSearchGrid(ClaimBatchGroupSearchGridRequestDto ClaimBatchGroupSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();

            return claimBatchingManagementService.GetAllClaimGroupForSearchGrid(
            ClaimBatchGroupSearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }


        [HttpPost]
        public object GetAllGroupsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();

            ClaimBatchGroupsRespondDto GroupData = claimBatchingManagementService.GetAllGroupsById(
            SecurityHelper.Context,
            AuditHelper.Context);
            return GroupData.ClaimBatchGroups.ToArray();
        }


        [HttpPost]
        public object AddClaimBatchGroup(JObject data)
        {
            try
            {
                data["Id"] = Guid.Empty;
                ClaimBatchGroupRequestDto ClaimBatchGroup = new ClaimBatchGroupRequestDto();
                ClaimBatchGroup = data.ToObject<ClaimBatchGroupRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimBatchGroupRequestDto result = claimBatchingManagementService.AddClaimBatchGroup(ClaimBatchGroup, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Claim Batch Group Added");
                if (result.ClaimBatchGroupEntryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Claim Batch Group failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Claim Batch Group failed!";
            }

        }

        [HttpPost]
        public object GetAllClaimBatchGroupById(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ClaimBatchGroupId = Guid.Parse(data["ClaimBatchGroupId"].ToString());
                IClaimInvoiceManagementService claimInvoiceManagementService = ServiceFactory.GetClaimInvoiceManagementService();
                Response = claimInvoiceManagementService.GetAllClaimBatchGroupById(ClaimBatchGroupId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllClaimDetailsByGroupID(JObject data)
        {
            try
            {
                Guid ClaimBatchGroup = Guid.Parse(data["ClaimBatchGroup"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimsResponseDto ClaimData = claimBatchingManagementService.GetAllClaimDetailsByGroupID(ClaimBatchGroup, SecurityHelper.Context,
                AuditHelper.Context);

                return ClaimData.Claims.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        public object GetAllClaimDetails()
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimsResponseDto ClaimData = claimBatchingManagementService.GetAllClaimDetails(SecurityHelper.Context,
                AuditHelper.Context);

                return ClaimData.Claims.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }


        [HttpPost]
        public object UpdateClaimBatchGroup(JObject data)
        {
            try
            {

                ClaimBatchGroupRequestDto ClaimBatchGroup = new ClaimBatchGroupRequestDto();
                ClaimBatchGroup = data.ToObject<ClaimBatchGroupRequestDto>();
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimBatchGroupRequestDto result = claimBatchingManagementService.UpdateClaimBatchGroup(ClaimBatchGroup, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Claim Batch Group Updated");
                if (result.ClaimBatchGroupEntryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Claim Batch Group failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Claim Batch Group failed!";
            }

        }

        [HttpPost]
        public object GetClaimBatchingById(JObject data)
        {
            try
            {
                Guid ClaimBatchGroup = Guid.Parse(data["Id"].ToString());                 
                IClaimBatchingManagementService claimBatchingManagementService = ServiceFactory.GetClaimBatchingManagementService();
                ClaimBatchingResponseDto result = claimBatchingManagementService.GetClaimBatchingById(ClaimBatchGroup, SecurityHelper.Context, AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "failed";
            }

        }
    }
}