using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ClaimManagementService : IClaimManagementService
    {
        public object UserValidationClaimSubmission(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            UserValidationClaimSubmissionUnitOfWork uow = new UserValidationClaimSubmissionUnitOfWork(loggedInUserId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetPoliciesForSearchGrid(PolicySearchInClaimSubmissionRequestDto claimPolicySearchRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            PolicySearchOnClaimSubmissionUnitOfWork uow = new PolicySearchOnClaimSubmissionUnitOfWork(claimPolicySearchRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetPoliciesNewForSearchGrid(PolicySearchInClaimSubmissionRequestDto claimPolicySearchRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            PolicySearchOnClaimSubmissionNewUnitOfWork uow = new PolicySearchOnClaimSubmissionNewUnitOfWork(claimPolicySearchRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetProductsForSearchGrid(ProductSearchInClaimSubmissionIloeRequestDto claimProductSearchRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ProductSearchOnClaimSubmissionIloeUnitOfWork uow = new ProductSearchOnClaimSubmissionIloeUnitOfWork(claimProductSearchRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }


        public object GetPolicyDetailsById(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            PolicyDetailsOnClaimSubmissionUnitOfWork uow = new PolicyDetailsOnClaimSubmissionUnitOfWork(policyId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllPartAreasByCommodityCategoryId(Guid commodityCategoryId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            PartAreasRetriveByCommodityCategoryIdUnitOfWork uow = new PartAreasRetriveByCommodityCategoryIdUnitOfWork(commodityCategoryId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllPartsByPartAreaMakeId(Guid partAreaId, Guid makeId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            PartsRetriveByPartAreaAndMakeIdUnitOfWork uow = new PartsRetriveByPartAreaAndMakeIdUnitOfWork(partAreaId, makeId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object ValidatePartInformation(Guid dealerId, Guid makeId, Guid partId, Guid modelId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ValidatePartInformationUnitOfWork uow = new ValidatePartInformationUnitOfWork(dealerId, makeId, partId, modelId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public string AddPart(PartRequestDto Part,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            string result = string.Empty;
            PartInsertionUnitOfWork uow = new PartInsertionUnitOfWork(Part);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }

        public string UpdatePart(PartRequestDto Part, SecurityContext securityContext, AuditContext auditContext)
        {
            string result = string.Empty;
            PartUpdationUnitOfWork uow = new PartUpdationUnitOfWork(Part);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }

            return result;
        }

        public PartAreaRequestDto AddPartArea(PartAreaRequestDto PartArea,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            PartAreaRequestDto result = new PartAreaRequestDto();
            PartAreaInsertionUnitOfWork uow = new PartAreaInsertionUnitOfWork(PartArea);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PartAreaInsertion = uow.PartArea.PartAreaInsertion;
            return result;
        }

        public PartAreaRequestDto UpdatePartArea(PartAreaRequestDto PartArea, SecurityContext securityContext, AuditContext auditContext)
        {
            PartAreaRequestDto result = new PartAreaRequestDto();
            PartAreaUpdationUnitOfWork uow = new PartAreaUpdationUnitOfWork(PartArea);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PartAreaInsertion = uow.PartArea.PartAreaInsertion;
            return result;
        }

        public PartArearsResponseDto GetAllPartArea(SecurityContext securityContext,
            AuditContext auditContext)
        {
            PartArearsResponseDto result = null;
            PartAreasRetrievalUnitOfWork uow = new PartAreasRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public PartAreaResponseDto GetPartAreaById(Guid PartAreaId, SecurityContext securityContext, AuditContext auditContext)
        {
            PartAreaResponseDto result = new PartAreaResponseDto();
            PartAreaByIdRetrievalUnitOfWork uow = new PartAreaByIdRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PartAreaId = PartAreaId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PartResponseDto GetPartById(Guid PartId, SecurityContext securityContext, AuditContext auditContext)
        {
            PartResponseDto result = new PartResponseDto();
            PartByIdRetrievalUnitOfWork uow = new PartByIdRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PartId = PartId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PartResponseDto GetPartByPartAreaId(Guid PartAreaId, SecurityContext securityContext, AuditContext auditContext)
        {
            PartResponseDto result = new PartResponseDto();
            PartByAreaIdRetrievalUnitOfWork uow = new PartByAreaIdRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PartAreaId = PartAreaId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetAllServiceHistoryByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ServiceHistoryByPolicyIdRetrievalUnitOfWork uow = new ServiceHistoryByPolicyIdRetrievalUnitOfWork(policyId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object AddServiceHistory(Guid policyId, ServiceHistoryRequestDto serviceData, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = false;
            AddServiceHistoryUnitOfWork uow = new AddServiceHistoryUnitOfWork(policyId, serviceData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object deleteServiceRecord(Guid policyId, ServiceHistoryRequestDto serviceData, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = false;
            DeleteServiceHistoryUnitOfWork uow = new DeleteServiceHistoryUnitOfWork(policyId, serviceData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllRelatedParts(Guid partId, Guid dealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = false;
            RelatedPartRetirvalByPartIdUnitOfWork uow = new RelatedPartRetirvalByPartIdUnitOfWork(partId, dealerId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllPriceForSearchGrid(PriceSearchGridRequestDto PriceSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PriceRetrievalForSearchGridUnitOfWork uow = new PriceRetrievalForSearchGridUnitOfWork(PriceSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public PartsResponseDto GetAllPartAreaByMakeId(Guid MakeId, SecurityContext securityContext, AuditContext auditContext)
        {
            PartsResponseDto result = null;

            PartAreaByMakeIdRetrievalUnitOfWork uow = new PartAreaByMakeIdRetrievalUnitOfWork(MakeId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }



        public object SubmitClaim(ClaimSubmissionRequestDto claimData, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            SubmitClaimUnitOfWork uow = new SubmitClaimUnitOfWork(claimData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object SubmitOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimData, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            SubmitOtherTireClaimUnitOfWork uow = new SubmitOtherTireClaimUnitOfWork(claimData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object UpdateClaim(ClaimUpdateRequestDto claimData, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            UpdateClaimUnitOfWork uow = new UpdateClaimUnitOfWork(claimData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object UpdateOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimData, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            UpdateOtherTireClaimUnitOfWork uow = new UpdateOtherTireClaimUnitOfWork(claimData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }


        public object GetAllPartAreasByCommodityTypeId(Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            PartAreasRetrivlByCommodityTypeIdUnitOfWork uow = new PartAreasRetrivlByCommodityTypeIdUnitOfWork(CommodityTypeId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllPartsByMakePartArea(Guid PartAreaId, Guid MakeId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            PartRetrivalByMakePartAreaUnitOfWork uow = new PartRetrivalByMakePartAreaUnitOfWork(PartAreaId, MakeId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllRelatedPartsByPartId(Guid partId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            PartSuggestionsByPartIdRetrivalUnitOfWork uow = new PartSuggestionsByPartIdRetrivalUnitOfWork(partId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public string AddPartSuggestion(PartSuggestionRequestDto PartSuggestionRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            string Response = string.Empty;
            SavePartSuggestionsUnitOfWork uow = new SavePartSuggestionsUnitOfWork(PartSuggestionRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object UserValidationClaimListing(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            UserValidationClaimListingUnitOfWork uow = new UserValidationClaimListingUnitOfWork(loggedInUserId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;

        }

        public object GetAllSubmittedClaimsByUserId(ClaimListRequestDto claimListRequestDto, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            SubmittedClaimRetrievalUnitOfWork uow = new SubmittedClaimRetrievalUnitOfWork(claimListRequestDto);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }
        public object GetAllSubmittedClaimsForEditByUserId(ClaimListRequestDto claimListRequestDto, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            SubmittedClaimForEditRetrievalUnitOfWork uow = new SubmittedClaimForEditRetrievalUnitOfWork(claimListRequestDto);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }


        public object GetClaimRequestDetailsByClaimId(Guid claimId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            ClaimRetrievalByIdUnitOfWork uow = new ClaimRetrievalByIdUnitOfWork(claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object LoadClaimDetailsForClaimEdit(Guid claimId, Guid userId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            ClaimRetrievalForEditByIdUnitOfWork uow = new ClaimRetrievalForEditByIdUnitOfWork(userId, claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }


        public PartRejectionTypeResponseDto GetAllPartRejectionDescription(SecurityContext securityContext,
            AuditContext auditContext)
        {
            PartRejectionTypeResponseDto result = null;
            PartRejectionTypeRetrievalUnitOfWork uow = new PartRejectionTypeRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }


        public PartRejectionTypeRequestDto AddPartRejection(PartRejectionTypeRequestDto PartRejectionType,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            PartRejectionTypeRequestDto result = new PartRejectionTypeRequestDto();
            PartRejectionTypeInsertionUnitOfWork uow = new PartRejectionTypeInsertionUnitOfWork(PartRejectionType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PartRejectionTypeInsertion = uow.PartRejectionType.PartRejectionTypeInsertion;
            return result;
        }

        public PartRejectionTypesResponseDto GetPartRejectionTypeById(Guid PartRejectionId, SecurityContext securityContext, AuditContext auditContext)
        {
            PartRejectionTypesResponseDto result = new PartRejectionTypesResponseDto();
            PartRejectionTypeByIdRetrievalUnitOfWork uow = new PartRejectionTypeByIdRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PartRejectionTypeId = PartRejectionId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PartRejectionTypeRequestDto UpdatePartRejectioDescription(PartRejectionTypeRequestDto PartArea, SecurityContext securityContext, AuditContext auditContext)
        {
            PartRejectionTypeRequestDto result = new PartRejectionTypeRequestDto();
            PartRejectionTypeUpdationUnitOfWork uow = new PartRejectionTypeUpdationUnitOfWork(PartArea);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PartRejectionTypeInsertion = uow.PartRejectionType.PartRejectionTypeInsertion;
            return result;
        }

        public object GetAllClaimsToProcessByUserId(ClaimRetirevalForProcessRequestDto claimRequestData,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ClaimListRetrievalForProcessUnitOfWork uow = new ClaimListRetrievalForProcessUnitOfWork(claimRequestData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }


        public object ValidateClaim(ValidateClaimProcessRequestDto validateClaimProcessRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ClaimProcessValidationUnitOfWork uow = new ClaimProcessValidationUnitOfWork(validateClaimProcessRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object SavePartWithClaim(PartWithClaimRequestDto partDetails, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            SavePartWithClaimUnitOfWork uow = new SavePartWithClaimUnitOfWork(partDetails);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }


        public object GetPolicyDetailsForClaimProcess(Guid policyId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            GetPolicyDetailsByPolicyIdToClaimProcessUnitOfWork uow = new GetPolicyDetailsByPolicyIdToClaimProcessUnitOfWork(policyId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;

        }

        #region < ClaimChequePayment >

        public object GetPendingClaimGroupsByDealer(Guid countryId, Guid dealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            List<ClaimChequePaymentDetailResponseDto> result = new List<ClaimChequePaymentDetailResponseDto>();
            PendingClaimGroupsRetrievalByDealerUnitOfWork uow = new PendingClaimGroupsRetrievalByDealerUnitOfWork(countryId, dealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool AddClaimChequePayment(ClaimChequePaymentRequestDto chequeData, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            ClaimChequePaymentInsertionUnitOfWork uow = new ClaimChequePaymentInsertionUnitOfWork(chequeData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.result;
            return result;
        }

        public bool ClaimChequeNoIsExists(string chequeNo, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            ClaimChequeNoIsExistsUnitOfWork uow = new ClaimChequeNoIsExistsUnitOfWork(chequeNo);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetAllChequeSearchGrid(ChequeSearchGridRequestDto ChequeSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimChequeRetrievalForSearchGridUnitOfWork uow = new ClaimChequeRetrievalForSearchGridUnitOfWork(ChequeSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public byte[] GetChequeAttachmentById(Guid chequePaymentId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimChequeGetAttachmentByIdUnitOfWork uow = new ClaimChequeGetAttachmentByIdUnitOfWork(chequePaymentId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        #endregion

        public object SaveClaimItem(ClamItemRequestDto clamItemRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            ClaimItemInsertionUnitOfWork uow = new ClaimItemInsertionUnitOfWork(clamItemRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object SaveClaimEngineerComment(SaveClaimEngineerCommentRequestDto clamEngineerRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            ClaimEngineerCommentInsertionUnitOfWork uow = new ClaimEngineerCommentInsertionUnitOfWork(clamEngineerRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object SaveClaimWithLabourCharge(AddLabourChargeToClaimRequestDto labourChargeToClaimRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            ClaimWithLabourChargeInsertionUnitOfWork uow = new ClaimWithLabourChargeInsertionUnitOfWork(labourChargeToClaimRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object ProcessClaim(ProcessClaimRequestDto claimProcessRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object result = null;
            ProcessClaimUnitOfWork uow = new ProcessClaimUnitOfWork(claimProcessRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetPolicyDetailsForView(GetPolicyDetailsForViewInClaimRequestDto getPolicyDetailsForViewRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            GetPolicyDetailsForViewUnitOfWork uow = new GetPolicyDetailsForViewUnitOfWork(getPolicyDetailsForViewRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetClaimDetailsForView(GetPolicyDetailsForViewInClaimRequestDto getClaimDetailsForViewRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            GetClaimDetailsForViewUnitOfWork uow = new GetClaimDetailsForViewUnitOfWork(getClaimDetailsForViewRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetAllClaimForSearchGrid(
            ClaimSearchGridRequestDto ClaimSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimRetrievalForSearchGridUnitOfWork uow = new ClaimRetrievalForSearchGridUnitOfWork(ClaimSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }


        public object GetClaimDetilsByClaimId(Guid claimId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            ClaimRetrievalByIdUnitOfWork uow = new ClaimRetrievalByIdUnitOfWork(claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        //public ClaimEndorsementPartDetailsRequestDto SavePartWithClaimEndorsement(ClaimEndorsementPartDetailsRequestDto partDetails,
        //SecurityContext securityContext,
        //AuditContext auditContext)
        //{
        //    object response = new object();
        //    ClaimEndorsementPartDetailsRequestDto result = new ClaimEndorsementPartDetailsRequestDto();
        //    SavePartWithClaimEndorsementUnitOfWork uow = new SavePartWithClaimEndorsementUnitOfWork(partDetails);
        //    uow.SecurityContext = securityContext;
        //    uow.AuditContext = auditContext;

        //    if (uow.PreExecute())
        //    {
        //        uow.Execute();
        //        response = uow.Result;
        //    }

        //    return result;
        //}

        //public object SaveClaimEndorsementWithLabourCharge(AddLabourChargeToClaimEndorsementRequestDto labourChargeToClaimRequest,
        //    SecurityContext securityContext, AuditContext auditContext)
        //{
        //    object result = null;
        //    ClaimEndorsementWithLabourChargeInsertionUnitOfWork uow = new ClaimEndorsementWithLabourChargeInsertionUnitOfWork(labourChargeToClaimRequest);
        //    uow.SecurityContext = securityContext;
        //    uow.AuditContext = auditContext;
        //    if (uow.PreExecute())
        //    {
        //        uow.Execute();
        //    }
        //    result = uow.Result;
        //    return result;
        //}

        public object ProcessClaimEndorsement(ProcessClaimEndorsementRequestDto claimEndorsementProcessRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object result = null;
            ProcessClaimEndorsementUnitOfWork uow = new ProcessClaimEndorsementUnitOfWork(claimEndorsementProcessRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object SaveClaimEndorsement(SaveClaimEndorsementProcessRequestDto SaveClaimEndorsementProcessRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            ClaimEndorsementInsertionUnitOfWork uow = new ClaimEndorsementInsertionUnitOfWork(SaveClaimEndorsementProcessRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetDealerDiscountsByScheme(DealerDiscountForClaimRequestDto dealerDiscountRequestDto,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            DealerDiscountRetrievalByClaimDetailsUnitOfWork uow = new DealerDiscountRetrievalByClaimDetailsUnitOfWork(dealerDiscountRequestDto);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllClaimForClaimInquirySearchGrid(
           ClaimInquirySearchGridRequestDto ClaimInquirySearchGridRequestDto,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            ClaimRetrievalForClaimInquirySearchGridUnitOfWork uow = new ClaimRetrievalForClaimInquirySearchGridUnitOfWork(ClaimInquirySearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public object GetClaimDetailsforInquiryByClaimId(Guid claimId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            ClaimforInquiryRetrievalByIdUnitOfWork uow = new ClaimforInquiryRetrievalByIdUnitOfWork(claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetExcitingClaimEndorsementDetails(Guid claimId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            GetExcitingEndrosmentByClaimIdUnitOfWork uow = new GetExcitingEndrosmentByClaimIdUnitOfWork(claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;

        }

        public object GetOldandNewClaimEndorsementDetails(Guid cliamEndrosmentId, Guid claimId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            GetOldandNewEndrosmentByEndrosmentIdUnitOfWork uow = new GetOldandNewEndrosmentByEndrosmentIdUnitOfWork(cliamEndrosmentId, claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;

        }

        public object ValidateClaimIdForEndorsement(ValidateClaimProcessRequestDto validateClaimEndorsementRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ValidateClaimEndorsementUnitOfWork uow = new ValidateClaimEndorsementUnitOfWork(validateClaimEndorsementRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }


        public object EndorseClaim(ClaimEndorseRequestDto claimEndorseRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            EndoresClaimUnitOfWork uow = new EndoresClaimUnitOfWork(claimEndorseRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object ValidatePolicyNumberOnClaimSubmission(string policyNumber, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object response = new object();
            ValidatePolicyNumberOnClaimSubmissionUnitOfWork uow = new ValidatePolicyNumberOnClaimSubmissionUnitOfWork(policyNumber);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object GetClaimDetailsByPolicyIdAndClaimId(Guid policyId, Guid claimId, string type, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            //ValidatePolicyNumberOnClaimSubmissionUnitOfWork uow = new ValidatePolicyNumberOnClaimSubmissionUnitOfWork(policyId);
            //uow.AuditContext = auditContext;
            //uow.SecurityContext = securityContext;
            //if (uow.PreExecute())
            //{
            //    uow.Execute();
            //    response = uow.Result;
            //}
            return response;
        }

        public object ValidateVinNumber(string vinNumber, Guid commodityTypeId, Guid dealerId, Guid productId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ValidateVinNumberUnitOfWork uow = new ValidateVinNumberUnitOfWork(vinNumber, commodityTypeId, dealerId,productId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object ValidateVinSerialNumberAndGetPolicyDetails(string vinNumber, Guid commodityTypeId, Guid dealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ValidateVinSerialNumberAndGetPolicyDetailsUnitOfWork uow = new ValidateVinSerialNumberAndGetPolicyDetailsUnitOfWork(vinNumber, commodityTypeId, dealerId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object SaveNewPart(AddNewPartByDealerRequestDto partSaveRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            AddNewPartByDealerUnitOfWork uow = new AddNewPartByDealerUnitOfWork(partSaveRequest);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object GetAllClaimStatus(SecurityContext securityContext, AuditContext auditContext)
        {

            object response = new object();
            GetAllClaimStatusResponseUnitOfWork uow = new GetAllClaimStatusResponseUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }
        public AttachmentsResponseDto GetClaimAndPolicyAttachmentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyAndClaimAttachmentsRetrivalByPolicyIdUnitOfWork uow = new PolicyAndClaimAttachmentsRetrivalByPolicyIdUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public AttachmentsResponseDto GetClaimAndPolicyAttachmentsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyAndClaimAttachmentsRetrivalByClaimIdUnitOfWork uow = new PolicyAndClaimAttachmentsRetrivalByClaimIdUnitOfWork(claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string AddNotes(NotesRequestDto Notes,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            string result = string.Empty;
            NotesInsertionUnitOfWork uow = new NotesInsertionUnitOfWork(Notes);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }

        public NotesResponseDto GetClaimNotesPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimNotesRetrivalByPolicyIdUnitOfWork uow = new ClaimNotesRetrivalByPolicyIdUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string AddComments(ClaimCommentRequestDto ClaimComments,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            string result = string.Empty;
            ClaimCommentInsertionUnitOfWork uow = new ClaimCommentInsertionUnitOfWork(ClaimComments);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }

        public object SaveAttachmentsToClaim(SaveAttachmentsToClaimRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            AttachmentsToClaimInsertionUnitOfWork uow = new AttachmentsToClaimInsertionUnitOfWork(requestData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }

        public object GoodwillAuthorizationByUserId(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            GoodwillAuthorizationByUserIdUnitOfWork uow = new GoodwillAuthorizationByUserIdUnitOfWork(loggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }

        public object GetAllOriginalPolicyDetailsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            GetAllOriginalPolicyDetailsByClaimIdUnitOfWork uow = new GetAllOriginalPolicyDetailsByClaimIdUnitOfWork(claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }
        public ClaimCommentsResponseDto GetClaimPendingCommentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimCommentsRetrivalByPolicyIdUnitOfWork uow = new ClaimCommentsRetrivalByPolicyIdUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllClaimHistoryDetailsByClaimId(Guid claimId, Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            AllClaimHistoryDetailsByClaimIdRetrievalUnitOfWork uow = new AllClaimHistoryDetailsByClaimIdRetrievalUnitOfWork(claimId, loggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ClaimInformationRequest(ClaimInformationRequestDto claimInformationRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimInformationRequestUnitOfWork uow = new ClaimInformationRequestUnitOfWork(claimInformationRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllClaimRejectionTypes(SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimRejectionTypesRetrievalUnitOfWork uow = new ClaimRejectionTypesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object RejectClaimRequest(ClaimRejectionRequestDto claimRejectionRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimRequestRejectionUnitOfWork uow = new ClaimRequestRejectionUnitOfWork(claimRejectionRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public object ViewClaimUpdateStatus(Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimRequestViewUpdateUnitOfWork uow = new ClaimRequestViewUpdateUnitOfWork(claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object UpdateClaimUpdateStatus(Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimRequestUpdateClaimUnitOfWork uow = new ClaimRequestUpdateClaimUnitOfWork(claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetTyreDetailsByPolicyNumber(string policyNumber, Guid commodityTypeId, Guid dealerId, Guid userId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            GetTyreDetailsByPolicyNumberUnitOfWork uow = new GetTyreDetailsByPolicyNumberUnitOfWork(policyNumber, commodityTypeId, dealerId, userId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object ValidateInvoiceCode(string invoiceCode, Guid commodityTypeId, Guid dealerId, Guid userId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ValidateInvoiceCodeUnitOfWork uow = new ValidateInvoiceCodeUnitOfWork(invoiceCode, commodityTypeId, dealerId, userId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }

        public object GetAllCustomerComplaints(SecurityContext securityContext, AuditContext auditContext)
        {
            CustomerComplaintsRetrievalUnitOfWork uow = new CustomerComplaintsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllDealerComments(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerCommentRetrievalUnitOfWork uow = new DealerCommentRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object LoadClaimDetailsForClaimEditOtherTire(Guid claimId, Guid userId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            object Response = string.Empty;
            ClaimRetrievalForEditByIdOtherTireUnitOfWork uow = new ClaimRetrievalForEditByIdOtherTireUnitOfWork(userId, claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        //public byte[] DownloadClaimAuthorizationForm(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        //{
        //    byte[] Response = null;
        //    ClaimAuthorizationFormExportUnitOfWork uow = new ClaimAuthorizationFormExportUnitOfWork(loggedInUserId, claimId);
        //    uow.AuditContext = auditContext;
        //    uow.SecurityContext = securityContext;
        //    if (uow.PreExecute())
        //    {
        //        uow.Execute();
        //        Response = uow.Result;
        //    }
        //    return Response;
        //}
        public UTDResponseDto GetCalculateUDT(Guid claimId, Guid policyId, Guid partId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetCalculateUDTvalueUnitOfWork uow = new GetCalculateUDTvalueUnitOfWork(claimId, policyId, partId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object DownloadClaimAuthorizationFormforTYER(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetClaimAuthorizationFormforTYERUnitOfWork uow = new GetClaimAuthorizationFormforTYERUnitOfWork(loggedInUserId, claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object DownloadClaimAuthorizationFormforCycle(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimAuthorizationFormExportUnitOfWork uow = new ClaimAuthorizationFormExportUnitOfWork(loggedInUserId, claimId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllClaimDashboardStatus(SecurityContext securityContext, AuditContext auditContext)
        {

            object response = new object();
            GetAllClaimDashboardStatusResponseUnitOfWork uow = new GetAllClaimDashboardStatusResponseUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }


    }
}
