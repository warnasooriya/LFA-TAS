using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IClaimManagementService
    {
        object UserValidationClaimSubmission(Guid loggedInUserId,
            SecurityContext securityContext, AuditContext auditContext);

        object GetPoliciesForSearchGrid(DataTransfer.Requests.PolicySearchInClaimSubmissionRequestDto claimPolicySearchRequest, SecurityContext securityContext, AuditContext auditContext);
        object GetPoliciesNewForSearchGrid(DataTransfer.Requests.PolicySearchInClaimSubmissionRequestDto claimPolicySearchRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetPolicyDetailsById(Guid policyId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPartAreasByCommodityCategoryId(Guid commodityCategoryId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPartsByPartAreaMakeId(Guid partAreaId, Guid makeId, SecurityContext securityContext, AuditContext auditContext);

        object ValidatePartInformation(Guid dealerId, Guid makeId, Guid partId, Guid modelId, SecurityContext securityContext, AuditContext auditContext);

        PartAreaRequestDto AddPartArea(PartAreaRequestDto PartArea, SecurityContext securityContext, AuditContext auditContext);

        PartArearsResponseDto GetAllPartArea(SecurityContext securityContext, AuditContext auditContext);

        PartAreaResponseDto GetPartAreaById(Guid PartAreaId, SecurityContext securityContext, AuditContext auditContext);

        PartAreaRequestDto UpdatePartArea(PartAreaRequestDto PartArea, SecurityContext securityContext, AuditContext auditContext);

        string AddPart(PartRequestDto Part, SecurityContext securityContext, AuditContext auditContext);


        object GetAllRelatedParts(Guid partId, Guid dealerId, SecurityContext securityContext, AuditContext auditContext);

        object AddServiceHistory(Guid policyId, ServiceHistoryRequestDto serviceData, SecurityContext securityContext, AuditContext auditContext);

        object deleteServiceRecord(Guid policyId, ServiceHistoryRequestDto serviceData, SecurityContext securityContext, AuditContext auditContext);


        object GetAllServiceHistoryByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);

        string UpdatePart(PartRequestDto Part, SecurityContext securityContext, AuditContext auditContext);

        PartResponseDto GetPartByPartAreaId(Guid PartAreaId, SecurityContext securityContext, AuditContext auditContext);




        PartsResponseDto GetAllPartAreaByMakeId(Guid MakeId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPriceForSearchGrid(PriceSearchGridRequestDto PriceSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        PartResponseDto GetPartById(Guid PartId, SecurityContext securityContext, AuditContext auditContext);

        object SubmitClaim(ClaimSubmissionRequestDto claimData, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPartAreasByCommodityTypeId(Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPartsByMakePartArea(Guid PartAreaId, Guid MakeId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllRelatedPartsByPartId(Guid partId, SecurityContext securityContext, AuditContext auditContext);

        string AddPartSuggestion(PartSuggestionRequestDto PartSuggestionRequest, SecurityContext securityContext, AuditContext auditContext);

        object UserValidationClaimListing(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllSubmittedClaimsByUserId(ClaimListRequestDto claimListRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetClaimRequestDetailsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object LoadClaimDetailsForClaimEdit(Guid claimId, Guid userId, SecurityContext securityContext, AuditContext auditContext);

        object UpdateClaim(ClaimUpdateRequestDto claimData, SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimsToProcessByUserId(ClaimRetirevalForProcessRequestDto claimRequestData,
            SecurityContext securityContext, AuditContext auditContext);

        bool ClaimChequeNoIsExists(string chequeNo, SecurityContext securityContext, AuditContext auditContext);

        object GetPendingClaimGroupsByDealer(Guid countryId, Guid dealerId, SecurityContext securityContext, AuditContext auditContext);
        object GetProductsForSearchGrid(ProductSearchInClaimSubmissionIloeRequestDto claimPolicySearchRequest, SecurityContext securityContext, AuditContext auditContext);
        bool AddClaimChequePayment(ClaimChequePaymentRequestDto chequeData, SecurityContext securityContext, AuditContext auditContext);

        object GetAllChequeSearchGrid(ChequeSearchGridRequestDto ChequeSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        PartRejectionTypeResponseDto GetAllPartRejectionDescription(SecurityContext securityContext, AuditContext auditContext);

        PartRejectionTypeRequestDto AddPartRejection(PartRejectionTypeRequestDto PartRejectionType, SecurityContext securityContext, AuditContext auditContext);

        PartRejectionTypesResponseDto GetPartRejectionTypeById(Guid PartRejectionId, SecurityContext securityContext, AuditContext auditContext);

        PartRejectionTypeRequestDto UpdatePartRejectioDescription(PartRejectionTypeRequestDto PartRejectionTypes, SecurityContext securityContext, AuditContext auditContext);

        byte[] GetChequeAttachmentById(Guid chequePaymentId, SecurityContext securityContext, AuditContext auditContext);

        object SaveClaimItem(ClamItemRequestDto clamItemRequest, SecurityContext securityContext, AuditContext auditContext);

        object ValidateClaim(ValidateClaimProcessRequestDto vlaidaClaimProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        object SavePartWithClaim(PartWithClaimRequestDto partDetails, SecurityContext securityContext, AuditContext auditContext);

        object GetPolicyDetailsForClaimProcess(Guid policyId, SecurityContext securityContext, AuditContext auditContext);

        object SaveClaimEngineerComment(SaveClaimEngineerCommentRequestDto clamEngineerRequest, SecurityContext securityContext, AuditContext auditContext);

        object SaveClaimWithLabourCharge(AddLabourChargeToClaimRequestDto LabourChargeToClaimRequest, SecurityContext securityContext, AuditContext auditContext);

        object ProcessClaim(ProcessClaimRequestDto claimProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetPolicyDetailsForView(GetPolicyDetailsForViewInClaimRequestDto GetPolicyDetailsForViewRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetClaimDetailsForView(GetPolicyDetailsForViewInClaimRequestDto getClaimDetailsForViewRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetClaimDetilsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

       //object SaveClaimEndorsementWithLabourCharge(AddLabourChargeToClaimEndorsementRequestDto LabourChargeToClaimRequest, SecurityContext securityContext, AuditContext auditContext);

        object ProcessClaimEndorsement(ProcessClaimEndorsementRequestDto claimEndorsementProcessRequest, SecurityContext securityContext, AuditContext auditContext);





       // ClaimEndorsementPartDetailsRequestDto SavePartWithClaimEndorsement(ClaimEndorsementPartDetailsRequestDto CE, SecurityContext securityContext, AuditContext auditContext);

        object SaveClaimEndorsement(SaveClaimEndorsementProcessRequestDto SaveClaimEndorsementProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        //object SaveClaimEndorsement(SaveClaimEndorsementProcessRequestDto SaveClaimEndorsementProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetDealerDiscountsByScheme(DealerDiscountForClaimRequestDto dealerDiscountRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimForClaimInquirySearchGrid(ClaimInquirySearchGridRequestDto ClaimInquirySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetClaimDetailsforInquiryByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object GetExcitingClaimEndorsementDetails(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        //object GetOldandNewClaimEndorsementDetails(Guid cliamEndrosmentId, SecurityContext securityContext, AuditContext auditContext);

        object GetOldandNewClaimEndorsementDetails(Guid cliamEndrosmentId, Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object ValidateClaimIdForEndorsement(ValidateClaimProcessRequestDto validateClaimEndorsementRequest, SecurityContext securityContext, AuditContext auditContext);

        object EndorseClaim(ClaimEndorseRequestDto claimEndorseRequest, SecurityContext securityContext, AuditContext auditContext);

        object ValidatePolicyNumberOnClaimSubmission(string policyNumber, SecurityContext context, AuditContext auditContext);

        object GetClaimDetailsByPolicyIdAndClaimId(Guid policyId, Guid claimId,string type, SecurityContext context, AuditContext auditContext);
        object ValidateVinNumber(string vinNumber,Guid commodityTypeId,Guid dealerId , Guid productId, SecurityContext context, AuditContext auditContext);
        object SaveNewPart(AddNewPartByDealerRequestDto partSaveRequest, SecurityContext context, AuditContext auditContext);
        object GetAllClaimStatus(SecurityContext context, AuditContext auditContext);

        AttachmentsResponseDto GetClaimAndPolicyAttachmentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);
        AttachmentsResponseDto GetClaimAndPolicyAttachmentsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        string AddNotes(NotesRequestDto Notes, SecurityContext securityContext, AuditContext auditContext);

        NotesResponseDto GetClaimNotesPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);

        string AddComments(ClaimCommentRequestDto ClaimCommentRequest, SecurityContext securityContext, AuditContext auditContext);
        object SaveAttachmentsToClaim(SaveAttachmentsToClaimRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);
        object GoodwillAuthorizationByUserId(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllOriginalPolicyDetailsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        ClaimCommentsResponseDto GetClaimPendingCommentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllClaimHistoryDetailsByClaimId(Guid claimId, Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext);
        object ClaimInformationRequest(ClaimInformationRequestDto claimInformationRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object GetAllClaimRejectionTypes(SecurityContext securityContext, AuditContext auditContext);
        object RejectClaimRequest(ClaimRejectionRequestDto claimRejectionRequest, SecurityContext securityContext, AuditContext auditContext);
        object ViewClaimUpdateStatus(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object UpdateClaimUpdateStatus(Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object ValidateInvoiceCode(string invoiceCode, Guid commodityTypeId, Guid dealerId,Guid userId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllCustomerComplaints(SecurityContext securityContext, AuditContext auditContext);

        object GetAllDealerComments(SecurityContext securityContext, AuditContext auditContext);

        object SubmitOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimData, SecurityContext securityContext, AuditContext auditContext);

        object LoadClaimDetailsForClaimEditOtherTire(Guid claimId, Guid userId, SecurityContext securityContext, AuditContext auditContext);

        object UpdateOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimData, SecurityContext securityContext, AuditContext auditContext);
       // byte[] DownloadClaimAuthorizationForm(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        UTDResponseDto GetCalculateUDT(Guid claimId, Guid policyId, Guid partId, SecurityContext securityContext, AuditContext auditContext);
        object DownloadClaimAuthorizationFormforTYER(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext);

        object DownloadClaimAuthorizationFormforCycle(Guid loggedInUserId, Guid claimId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllClaimDashboardStatus(SecurityContext securityContext, AuditContext auditContext);
        object GetAllSubmittedClaimsForEditByUserId(ClaimListRequestDto claimListRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object ValidateVinSerialNumberAndGetPolicyDetails(string vinNumber, Guid commodityTypeId, Guid dealerId, SecurityContext context1, AuditContext context2);
        object GetTyreDetailsByPolicyNumber(string policyNumber, Guid commodityTypeId, Guid dealerId, Guid userId, SecurityContext context1, AuditContext context2);
    }
}
