using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Notification;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    [RoutePrefix("api/claim")]
    public class ClaimController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        ///     Validate logged in user can have access on this page
        /// </summary>
        /// <param name="data">contains logged in user id</param>
        /// <returns>user type code</returns>
        [HttpPost]
        [Route("UserValidationClaimSubmission")]
        public object UserValidationClaimSubmission(JObject data)
        {
            object Response = null;
            try
            {
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.UserValidationClaimSubmission(loggedInUserId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        [Route("UserValidationClaimListing")]
        public object UserValidationClaimListing(JObject data)
        {
            object Response = null;
            try
            {
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.UserValidationClaimListing(loggedInUserId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        /// <summary>
        ///     Policy search in claim submission
        /// </summary>
        /// <param name="data">Multiple db objects</param>
        /// <returns>Grid data object</returns>
        [HttpPost]
        [Route("GetPoliciesForSearchGrid")]
        public object GetPoliciesForSearchGrid(JObject data)
        {
            object Response = null;
            try
            {
                var claimPolicySearchRequest = data.ToObject<PolicySearchInClaimSubmissionRequestDto>();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetPoliciesForSearchGrid(claimPolicySearchRequest,
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


        // policy search for claim submission -> new functionality
        [HttpPost]
        [Route("GetPoliciesNewForSearchGrid")]
        public object GetPoliciesNewForSearchGrid(JObject data)
        {
            object Response = null;
            try
            {
                var claimPolicySearchRequest = data.ToObject<PolicySearchInClaimSubmissionRequestDto>();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetPoliciesNewForSearchGrid(claimPolicySearchRequest,
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
        [Route("GetProductsForSearchGrid")]
        public object GetProductsForSearchGrid(JObject data)
        {
            object Response = null;
            try
            {
                var claimPolicySearchRequest = data.ToObject<ProductSearchInClaimSubmissionIloeRequestDto>();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetProductsForSearchGrid(claimPolicySearchRequest,
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

        /// <summary>
        ///     get policy information for claim
        /// </summary>
        /// <param name="data">contains policyId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReadPolicyInformation")]
        public object ReadPolicyInformation(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetPolicyDetailsById(policyId,
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

        /// <summary>
        ///     Get service history of vehicle/electronic or item by policy id
        /// </summary>
        /// <param name="data">policy id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllServiceHistoryByPolicyId")]
        public object PServiceHistoryByPolicyId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllServiceHistoryByPolicyId(policyId,
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

        /// <summary>
        ///     Get part areas according to commodity category
        /// </summary>
        /// <param name="data">commodityCategoryId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllPartAreasByCommodityCategoryId")]
        public object GetAllPartAreasByCommodityCategoryId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var commodityCategoryId = Guid.Parse(data["commodityCategoryId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllPartAreasByCommodityCategoryId(commodityCategoryId,
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


        /// <summary>
        ///     submitting claim request
        /// </summary>
        /// <param name="data">ClaimSubmissionRequestDto</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SubmitClaim")]
        public object SubmitClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimData = data["claimData"].ToObject<ClaimSubmissionRequestDto>();
                claimData.claimDate = DateTime.UtcNow;
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SubmitClaim(claimData,
                    SecurityHelper.Context, AuditHelper.Context);
                if (response.ToString().ToLower() == "ok")
                {
                    StaticState.ClaimListSyncState = Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("SubmitOtherTireClaim")]
        public object SubmitOtherTireClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimData = data["claimOtherData"].ToObject<ClaimSubmissionOtherTireRequestDto>();
                claimData.claimDate = DateTime.UtcNow;
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SubmitOtherTireClaim(claimData,
                    SecurityHelper.Context, AuditHelper.Context);
                if (response.ToString().ToLower() == "ok")
                {
                    StaticState.ClaimListSyncState = Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("UpdateClaim")]
        public object UpdateClaim(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimData = data["claimData"].ToObject<ClaimUpdateRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.UpdateClaim(claimData,
                    SecurityHelper.Context, AuditHelper.Context);
                if (Response.ToString().ToLower() == "ok")
                {
                    StaticState.ClaimListSyncState = Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object UpdateOtherTireClaim(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ClaimSubmissionOtherTireRequestDto ClaimOtherTireUpdateRequest = data["claimData"].ToObject<ClaimSubmissionOtherTireRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.UpdateOtherTireClaim(ClaimOtherTireUpdateRequest,
                    SecurityHelper.Context, AuditHelper.Context);
                if (Response.ToString().ToLower() == "ok")
                {
                    StaticState.ClaimListSyncState = Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("ValidateVinSerialNumber")]
        public object ValidateVinSerialNumber(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string vinNumber = data["vinNo"].ToString();
                Guid commodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());
                Guid productId = Guid.Parse(data["productId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.ValidateVinNumber(vinNumber, commodityTypeId, dealerId,productId,
                    SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("ValidateVinSerialNumberAndGetPolicyDetails")]
        public object ValidateVinSerialNumberAndGetPolicyDetails(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string vinNumber = data["vinNo"].ToString();
                Guid commodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.ValidateVinSerialNumberAndGetPolicyDetails(vinNumber, commodityTypeId, dealerId,
                    SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("ValidateInvoiceCode")]
        public object ValidateInvoiceCode(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string invoiceCode = data["vinNo"].ToString();
                Guid commodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());
                Guid userId = Guid.Parse(data["userId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.ValidateInvoiceCode(invoiceCode, commodityTypeId, dealerId, userId,
                    SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetTyreDetailsByPolicyNumber")]
        public object GetTyreDetailsByPolicyNumber(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                string policyNumber = data["policyNumber"].ToString();
                Guid commodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());
                Guid dealerId = Guid.Parse(data["dealerId"].ToString());
                Guid userId = Guid.Parse(data["userId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetTyreDetailsByPolicyNumber(policyNumber, commodityTypeId, dealerId, userId,
                    SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("SaveNewPart")]
        public object SaveNewPart(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                AddNewPartByDealerRequestDto partSaveRequest = data.ToObject<AddNewPartByDealerRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.SaveNewPart(partSaveRequest,
                    SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetAllClaimsToProcessByUserId")]
        public object GetAllClaimsToProcessByUserId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimRequestData = data["data"].ToObject<ClaimRetirevalForProcessRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllClaimsToProcessByUserId(claimRequestData,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        //yet to refactor
        [HttpPost]
        [Route("GetAllPartArea")]
        public object GetAllPartArea()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                var PartAreaData = claimManagementService.GetAllPartArea(SecurityHelper.Context,
                    AuditHelper.Context);

                return PartAreaData.PartArears.ToArray();
            }
            catch (Exception e)
            {
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return e.Message;
            }
        }

        //yet to refactor
        [HttpPost]
        [Route("GetPartAreaById")]
        public object GetPartAreaById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var claimManagementService = ServiceFactory.GetClaimManagementService();

            var PartArea = claimManagementService.GetPartAreaById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return PartArea;
        }

        //yet to refactor
        [HttpPost]
        [Route("GetPartById")]
        public object GetPartById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var claimManagementService = ServiceFactory.GetClaimManagementService();

            var Part = claimManagementService.GetPartById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Part;
        }

        //yet to refactor
        [HttpPost]
        [Route("GetPartByPartAreaId")]
        public object GetPartByPartAreaId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var claimManagementService = ServiceFactory.GetClaimManagementService();

            var Part = claimManagementService.GetPartByPartAreaId(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Part;
        }

        /// <summary>
        ///     Get All parts related to part area and make
        /// </summary>
        /// <param name="data">partAreaId & makeId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllPartsByPartAreaMakeId")]
        public object GetAllPartsByPartAreaMakeId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var partAreaId = Guid.Parse(data["partAreaId"].ToString());
                var makeId = Guid.Parse(data["makeId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllPartsByPartAreaMakeId(partAreaId, makeId,
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
        [Route("GetAllPartAreaByMakeId")]
        public object GetAllPartAreaByMakeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var claimManagementService = ServiceFactory.GetClaimManagementService();

            var PartData = claimManagementService.GetAllPartAreaByMakeId(Guid.Parse(data["MakeId"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return PartData.Part.ToArray();
        }

        [HttpPost]
        [Route("AddServiceHistory")]
        public object AddServiceHistory(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var serviceData = data["serviceData"].ToObject<ServiceHistoryRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.AddServiceHistory(policyId, serviceData,
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
        [Route("deleteServiceRecord")]
        public object deleteServiceRecord(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var serviceData = data["serviceData"].ToObject<ServiceHistoryRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.deleteServiceRecord(policyId, serviceData,
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
        [Route("ValidatePartInformation")]
        public object ValidatePartInformation(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var dealerId = Guid.Parse(data["dealerId"].ToString());
                var makeId = Guid.Parse(data["makeId"].ToString());
                var partId = Guid.Parse(data["partId"].ToString());
                var modelId = Guid.Parse(data["modelId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.ValidatePartInformation(dealerId, makeId, partId, modelId,
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
        [Route("GetAllRelatedParts")]
        public object GetAllRelatedParts(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var partId = Guid.Parse(data["partId"].ToString());
                var dealerId = Guid.Parse(data["dealerId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllRelatedParts(partId, dealerId,
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
        [Route("GetAllRelatedPartsByPartId")]
        public object GetAllRelatedPartsByPartId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var partId = Guid.Parse(data["partId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllRelatedPartsByPartId(partId,
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
        [Route("AddPart")]
        public string AddPart(JObject data)
        {
            var Response = string.Empty;
            try
            {
                var PartRequest = data.ToObject<PartRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.AddPart(PartRequest, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                Response = "Error occured while saving part";
            }
            return Response;
        }

        [HttpPost]
        [Route("UpdatePart")]
        public string UpdatePart(JObject data)
        {
            var Response = string.Empty;
            try
            {
                var PartRequest = data.ToObject<PartRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.UpdatePart(PartRequest, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                Response = "Error occured while updating part";
            }
            return Response;
        }

        [HttpPost]
        [Route("AddPartArea")]
        public object AddPartArea(JObject data)
        {
            var PartArea = data.ToObject<PartAreaRequestDto>();
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            var result = claimManagementService.AddPartArea(PartArea, SecurityHelper.Context, AuditHelper.Context);
            logger.Info("Part Area Added");
            if (result.PartAreaInsertion)
            {
                return "OK";
            }
            return "Add Part Area failed!";
        }

        [HttpPost]
        [Route("UpdatePartArea")]
        public string UpdatePartArea(JObject data)
        {
            //ILog logger = LogManager.GetLogger(typeof(ApiController));
            //logger.Debug("Add Part Area  method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var PartArea = data.ToObject<PartAreaRequestDto>();
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            var result = claimManagementService.UpdatePartArea(PartArea, SecurityHelper.Context, AuditHelper.Context);
            logger.Info("Part Area Added");
            if (result.PartAreaInsertion)
            {
                return "OK";
            }
            return "Update Part Area  failed!";
        }

        [HttpPost]
        [Route("GetAllPriceForSearchGrid")]
        public object GetAllPriceForSearchGrid(PriceSearchGridRequestDto PriceSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetAllPriceForSearchGrid(PriceSearchGridRequestDto, SecurityHelper.Context,
                AuditHelper.Context);
        }


        [HttpPost]
        [Route("GetAllPartAreasByCommodityTypeId")]
        public object GetAllPartAreasByCommodityTypeId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var CommodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllPartAreasByCommodityTypeId(CommodityTypeId,
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
        [Route("GetAllPartsByMakePartArea")]
        public object GetAllPartsByMakePartArea(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var PartAreaId = Guid.Parse(data["partAreaId"].ToString());
                var MakeId = Guid.Parse(data["makeId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetAllPartsByMakePartArea(PartAreaId, MakeId,
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
        [Route("SavePartSuggestion")]
        public string SavePartSuggestion(JObject data)
        {
            var Response = string.Empty;
            try
            {
                var PartSuggestionRequest = data.ToObject<PartSuggestionRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.AddPartSuggestion(PartSuggestionRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                Response = "Error occured while saving part";
            }
            return Response;
        }


        [HttpPost]
        [Route("GetAllSubmittedClaimsByUserId")]
        public object GetAllSubmittedClaimsByUserId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    var claimListRequestDto = data.ToObject<ClaimListRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllSubmittedClaimsByUserId(claimListRequestDto,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while getting submitted claims";
            }
            return response;
        }

        [HttpPost]
        [Route("GetAllSubmittedClaimsForEditByUserId")]
        public object GetAllSubmittedClaimsForEditByUserId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimListRequestDto = data.ToObject<ClaimListRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllSubmittedClaimsForEditByUserId(claimListRequestDto,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while getting submitted claims";
            }
            return response;
        }

        [HttpPost]
        [Route("GetClaimRequestByClaimId")]
        public object GetClaimRequestByClaimId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetClaimRequestDetailsByClaimId(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("ClaimInformationRequest")]
        public object ClaimInformationRequest(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimInformationRequestDto = data["claimInfoRequest"].ToObject<ClaimInformationRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.ClaimInformationRequest(claimInformationRequestDto, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }




        [HttpPost]
        [Route("GetAllClaimHistoryDetailsByClaimId")]
        public object GetAllClaimHistoryDetailsByClaimId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var loggedInUserId = Guid.Parse(data["LoggedInUserId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllClaimHistoryDetailsByClaimId(claimId, loggedInUserId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }


        [HttpPost]
        [Route("CalculateUDT")]
        public UTDResponseDto CalculateUDT(JObject data)
        {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var partId = Guid.Parse(data["partId"].ToString());


                var claimManagementService = ServiceFactory.GetClaimManagementService();
                return  claimManagementService.GetCalculateUDT(claimId, policyId,partId, SecurityHelper.Context,
                    AuditHelper.Context);

        }


        [HttpPost]
        [Route("GetAllOriginalPolicyDetailsByClaimId")]
        public object GetAllOriginalPolicyDetailsByClaimId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllOriginalPolicyDetailsByClaimId(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }



        [HttpPost]
        [Route("GetAllClaimRejectionTypes")]
        public object GetAllClaimRejectionTypes()
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllClaimRejectionTypes(SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("GetAllCustomerComplaints")]
        public object GetAllCustomerComplaints()
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllCustomerComplaints(SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("GetAllDealerComments")]
        public object GetAllDealerComments()
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllDealerComments(SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("RejectClaimRequest")]
        public object RejectClaimRequest(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimRejectionRequest = data.ToObject<ClaimRejectionRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.RejectClaimRequest(claimRejectionRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("ViewClaimUpdateStatus")]
        public object ViewClaimUpdateStatus(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.ViewClaimUpdateStatus(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }
        [HttpPost]
        [Route("ViewClaimUpdateStatus")]
        public object UpdateClaimUpdateStatus(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.UpdateClaimUpdateStatus(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("EndorseClaim")]
        public object EndorseClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimEndorseRequest = data.ToObject<ClaimEndorseRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.EndorseClaim(claimEndorseRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }


        [HttpPost]
        [Route("LoadClaimDetailsForClaimEdit")]
        public object LoadClaimDetailsForClaimEdit(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var userId = Guid.Parse(data["loggedInUserId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.LoadClaimDetailsForClaimEdit(claimId, userId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("LoadClaimDetailsForClaimEditOtherTire")]
        public object LoadClaimDetailsForClaimEditOtherTire(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var userId = Guid.Parse(data["loggedInUserId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.LoadClaimDetailsForClaimEditOtherTire(claimId, userId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("SavePartWithClaim")]
        public object SavePartWithClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var partDetails = data.ToObject<PartWithClaimRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SavePartWithClaim(partDetails, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("SaveClaimItem")]
        public object SaveClaimItem(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var clamItemRequest = data.ToObject<ClamItemRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.SaveClaimItem(clamItemRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("SaveClaimEngineerComment")]
        public object SaveClaimEngineerComment(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var clamEngineerRequest = data.ToObject<SaveClaimEngineerCommentRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SaveClaimEngineerComment(clamEngineerRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        [Route("SaveClaimWithLabourCharge")]
        public object SaveClaimWithLabourCharge(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var LabourChargeToClaimRequest = data.ToObject<AddLabourChargeToClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SaveClaimWithLabourCharge(LabourChargeToClaimRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        [Route("GetPolicyDetailsForClaimProcess")]
        public object GetPolicyDetailsForClaimProcess(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["data"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetPolicyDetailsForClaimProcess(policyId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("ValidateClaim")]
        public object ValidateClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var validateClaimProcessRequest = data.ToObject<ValidateClaimProcessRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.ValidateClaim(validateClaimProcessRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("ValidateClaimIdForEndorsement")]
        public object ValidateClaimIdForEndorsement(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var validateClaimEndorsementRequest = data.ToObject<ValidateClaimProcessRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.ValidateClaimIdForEndorsement(validateClaimEndorsementRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("ProcessClaim")]
        public object ProcessClaim(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimProcessRequest = data.ToObject<ProcessClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.ProcessClaim(claimProcessRequest, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        [Route("GetPolicyDetailsForView")]
        public object GetPolicyDetailsForView(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var GetPolicyDetailsForViewRequest = data.ToObject<GetPolicyDetailsForViewInClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetPolicyDetailsForView(GetPolicyDetailsForViewRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        [Route("GetClaimDetailsForView")]
        public object GetClaimDetailsForView(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var getClaimDetailsForViewRequest = data.ToObject<GetPolicyDetailsForViewInClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetClaimDetailsForView(getClaimDetailsForViewRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("GetItemDetForView")]
        public object GetItemDetForView(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var getClaimDetailsForViewRequest = data.ToObject<GetPolicyDetailsForViewInClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetClaimDetailsForView(getClaimDetailsForViewRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured while validating the claim";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("GetAllPartRejectioDescription")]
        public object GetAllPartRejectioDescription()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                var PartRejectionTypeData = claimManagementService.GetAllPartRejectionDescription(
                    SecurityHelper.Context,
                    AuditHelper.Context);

                return PartRejectionTypeData.PartRejectionType.ToArray();
            }
            catch (Exception e)
            {
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return e.Message;
            }
        }

        [HttpPost]
        [Route("AddPartRejection")]
        public object AddPartRejection(JObject data)
        {
            var PartRejectionType = data.ToObject<PartRejectionTypeRequestDto>();
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            var result = claimManagementService.AddPartRejection(PartRejectionType, SecurityHelper.Context,
                AuditHelper.Context);
            logger.Info("Part Rejection Added");
            if (result.PartRejectionTypeInsertion)
            {
                return "OK";
            }
            return "Add Part Rejection failed!";
        }

        [HttpPost]
        [Route("GetPartRejectionTypeById")]
        public object GetPartRejectionTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var claimManagementService = ServiceFactory.GetClaimManagementService();

            var PartRejectionTypes = claimManagementService.GetPartRejectionTypeById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return PartRejectionTypes;
        }

        [HttpPost]
        [Route("UpdatePartRejectioDescription")]
        public string UpdatePartRejectioDescription(JObject data)
        {
            //ILog logger = LogManager.GetLogger(typeof(ApiController));
            //logger.Debug("Add Part Area  method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var PartRejectionTypes = data.ToObject<PartRejectionTypeRequestDto>();
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            var result = claimManagementService.UpdatePartRejectioDescription(PartRejectionTypes, SecurityHelper.Context,
                AuditHelper.Context);
            logger.Info("Part Area Added");
            if (result.PartRejectionTypeInsertion)
            {
                return "OK";
            }
            return "Update Part Area  failed!";
        }

        [HttpPost]
        [Route("GetAllClaimForSearchGrid")]
        public object GetAllClaimForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();

            return claimManagementService.GetAllClaimForSearchGrid(
                ClaimSearchGridRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
        }

        [HttpPost]
        [Route("GetClaimDetilsByClaimId")]
        public object GetClaimDetilsByClaimId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetClaimDetilsByClaimId(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }


        //[HttpPost]
        //[Route("SaveClaimEndorsementWithLabourCharge")]
        //public object SaveClaimEndorsementWithLabourCharge(JObject data)
        //{
        //    object response = null;
        //    try
        //    {
        //        SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
        //        AddLabourChargeToClaimEndorsementRequestDto LabourChargeToClaimRequest = data.ToObject<AddLabourChargeToClaimEndorsementRequestDto>();
        //        IClaimManagementService claimManagementService = ServiceFactory.GetClaimManagementService();
        //        response = claimManagementService.SaveClaimEndorsementWithLabourCharge(LabourChargeToClaimRequest, SecurityHelper.Context, AuditHelper.Context);

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
        //    }
        //    return response;
        //}

        [HttpPost]
        [Route("ProcessClaimEndorsement")]
        public object ProcessClaimEndorsement(JObject data)
        {
            object response = null;

            try
            {
                var SaveClaimEndorsementProcessRequest = data.ToObject<SaveClaimEndorsementProcessRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SaveClaimEndorsement(SaveClaimEndorsementProcessRequest,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        [Route("GetDealerDiscountsByScheme")]
        public object GetDealerDiscountsByScheme(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var dealerDiscountRequestDto = data.ToObject<DealerDiscountForClaimRequestDto>();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetDealerDiscountsByScheme(dealerDiscountRequestDto,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("GetClaimDetailsByPolicyIdAndClaimId")]
        public object GetClaimDetailsByPolicyIdAndClaimId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyId = Guid.Parse(data["policyId"].ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var type = data["type"].ToString();

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetClaimDetailsByPolicyIdAndClaimId(policyId, claimId, type,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        [Route("GetAllClaimStatus")]
        public object GetAllClaimStatus()
        {
            object response = null;

            try
            {
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllClaimStatus(SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;

        }

        [HttpPost]
        [Route("GetAllClaimDashboardStatus")]
        public object GetAllClaimDashboardStatus()
        {
            object response = null;

            try
            {
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetAllClaimDashboardStatus(SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;

        }

        [HttpPost]
        [Route("SaveAttachmentsToClaim")]
        public object SaveAttachmentsToClaim(JObject data)
        {
            object response = null;
            try
            {
                var requestData = data.ToObject<SaveAttachmentsToClaimRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.SaveAttachmentsToClaim(requestData, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        [Route("GoodwillAuthorizationByUserId")]
        public object GoodwillAuthorizationByUserId(JObject data)
        {
            object response = null;
            try
            {
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GoodwillAuthorizationByUserId(loggedInUserId, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        //[HttpPost]
        //[Route("DownloadClaimAuthorizationForm")]
        //public HttpResponseMessage DownloadClaimAuthorizationForm(JObject data)
        //{
        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    byte[] array = null;
        //    try
        //    {
        //        var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
        //        var claimId = Guid.Parse(data["claimId"].ToString());

        //        var claimManagementService = ServiceFactory.GetClaimManagementService();
        //        array = claimManagementService.DownloadClaimAuthorizationForm(loggedInUserId, claimId, SecurityHelper.Context, AuditHelper.Context);

        //        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //        var filename = "ClaimsAuthorization_" + unixTimestamp + ".pdf";
        //        result.Content = new ByteArrayContent(array);
        //        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
        //        result.Content.Headers.ContentDisposition.FileName = filename;
        //        result.Content.Headers.Add("x-filename", filename);
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
        //                     ex.InnerException);
        //    }
        //    return result;
        //}


        [HttpPost]
        public object DownloadClaimAuthorizationForm(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                var result = claimManagementService.DownloadClaimAuthorizationFormforCycle(loggedInUserId, claimId,
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
        public object DownloadClaimAuthorizationFormforTYER(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                var result = claimManagementService.DownloadClaimAuthorizationFormforTYER(loggedInUserId, claimId,
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



        #region < ClaimChequePayment >

        [HttpPost]
        [Route("GetPendingClaimGroupsByDealer")]
        public object GetPendingClaimGroupsByDealer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                var countryId = Guid.Parse(data["countryId"].ToString());
                var dealerId = Guid.Parse(data["dealerId"].ToString());

                return claimManagementService.GetPendingClaimGroupsByDealer(countryId, dealerId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception e)
            {
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return e.Message;
            }
        }

        [HttpPost]
        [Route("AddClaimChequePayment")]
        public object AddClaimChequePayment(JObject data)
        {
            var chequeData = data.ToObject<ClaimChequePaymentRequestDto>();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            if (
                !claimManagementService.ClaimChequeNoIsExists(chequeData.ChequeNo, SecurityHelper.Context,
                    AuditHelper.Context))
            {
                var result = claimManagementService.AddClaimChequePayment(chequeData, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("Claim Cheque Payment Added");
                if (result)
                {
                    return "OK";
                }
                return "Save failed!";
            }
            return "Cheque No Is Already Exist !";
        }


        [HttpPost]
        [Route("ClaimChequeNoIsExists")]
        public object ClaimChequeNoIsExists(JObject data)
        {
            var chequeNo = data["chequeNo"].ToString();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.ClaimChequeNoIsExists(chequeNo, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllChequesForSearchGrid(ChequeSearchGridRequestDto ChequeSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetAllChequeSearchGrid(ChequeSearchGridRequestDto, SecurityHelper.Context,
                AuditHelper.Context);
        }

        [HttpPost]
        public HttpResponseMessage GetChequeAttachmentById(JObject data)
        {
            var chequePaymentId = Guid.Parse(data["chequePaymentId"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            var array = claimManagementService.GetChequeAttachmentById(chequePaymentId, SecurityHelper.Context,
                AuditHelper.Context);

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(array);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            result.Content.Headers.ContentDisposition.FileName = "Cheque.pdf";
            result.Content.Headers.Add("x-filename", "Cheque.pdf");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return result;
        }

        [HttpPost]
        public AttachmentsResponseDto GetClaimAndPolicyAttachmentsByPolicyId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetClaimAndPolicyAttachmentsByPolicyId(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public AttachmentsResponseDto GetClaimAndPolicyAttachmentsByClaimId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid claimId = Guid.Parse(data["Id"].ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetClaimAndPolicyAttachmentsByClaimId(claimId, SecurityHelper.Context, AuditHelper.Context);

        }
        //

        #endregion

        #region < ClaimInquiry >

        [HttpPost]
        [Route("GetAllClaimForClaimInquirySearchGrid")]
        public object GetAllClaimForClaimInquirySearchGrid(
            ClaimInquirySearchGridRequestDto ClaimInquirySearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();

            return claimManagementService.GetAllClaimForClaimInquirySearchGrid(
                ClaimInquirySearchGridRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
        }

        [HttpPost]
        [Route("GetClaimDetailsforInquiryByClaimId")]
        public object GetClaimDetailsforInquiryByClaimId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());

                var claimManagementService = ServiceFactory.GetClaimManagementService();
                response = claimManagementService.GetClaimDetailsforInquiryByClaimId(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                response = "Error occured while retriving claim information";
            }
            return response;
        }

        [HttpPost]
        [Route("GetExcitingClaimEndorsementDetails")]
        public object GetExcitingClaimEndorsementDetails(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var claimId = Guid.Parse(data["data"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetExcitingClaimEndorsementDetails(claimId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("GetOldandNewClaimEndorsementDetails")]
        public object GetOldandNewClaimEndorsementDetails(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var cliamEndrosmentId = Guid.Parse(data["CliamEndrosmentId"].ToString());
                var claimId = Guid.Parse(data["claimId"].ToString());
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.GetOldandNewClaimEndorsementDetails(cliamEndrosmentId, claimId,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        [Route("ValidatePolicyNumberOnClaimSubmission")]
        public object ValidatePolicyNumberOnClaimSubmission(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyNumber = data["policyNo"].ToString();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.ValidatePolicyNumberOnClaimSubmission(policyNumber,
                    SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }





        #endregion

        #region < Notes >

        [HttpPost]
        [Route("AddNotes")]
        public string AddNotes(JObject data)
        {
            var Response = string.Empty;
            try
            {
                var NotesRequest = data.ToObject<NotesRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.AddNotes(NotesRequest, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                Response = "Error occured while saving Notes";
            }
            return Response;
        }

        [HttpPost]
        public NotesResponseDto GetClaimNotesPolicyId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetClaimNotesPolicyId(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public ClaimCommentsResponseDto GetClaimPendingCommentsByPolicyId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            var claimManagementService = ServiceFactory.GetClaimManagementService();
            return claimManagementService.GetClaimPendingCommentsByPolicyId(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        #endregion

        #region < Comments >

        [HttpPost]
        [Route("AddComments")]
        public string AddComments(JObject data)
        {
            var Response = string.Empty;
            try
            {
                var ClaimCommentRequest = data.ToObject<ClaimCommentRequestDto>();
                var claimManagementService = ServiceFactory.GetClaimManagementService();
                Response = claimManagementService.AddComments(ClaimCommentRequest, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                Response = "Error occured while saving Comments";
            }
            return Response;
        }

        #endregion
    }
}