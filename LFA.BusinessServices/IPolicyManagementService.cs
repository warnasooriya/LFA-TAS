using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IPolicyManagementService
    {
        #region Policy
        PoliciesResponseDto GetPolicys(SecurityContext securityContext, AuditContext auditContext);
        PoliciesResponseDto GetPolicysByCustomerId(Guid customerid, SecurityContext securityContext, AuditContext auditContext);
        PolicyRequestDto AddPolicy(PolicyRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        PolicyResponseDto GetPolicyById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);
        PolicyRequestDto UpdatePolicy(PolicyRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        OnlinePurchaseRequestDto SaveOnlinePurchase(OnlinePurchaseRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Bundle
        PolicyBundlesResponseDto GetPolicyBundles(SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleRequestDto AddPolicyBundle(PolicyBundleRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleResponseDto GetPolicyBundleById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleRequestDto UpdatePolicyBundle(PolicyBundleRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Bundle History
        PolicyBundleHistoriesResponseDto GetPolicyBundleHistories(SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleHistoryRequestDto AddPolicyBundleHistory(PolicyBundleHistoryRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Bundle Transaction
        PolicyBundleTransactionsResponseDto GetPolicyBundleTransactions(SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleTransactionResponseDto GetPolicyBundleTransactionById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);
        PolicyBundleTransactionRequestDto AddPolicyBundleTransaction(PolicyBundleTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Cancellation Bundle
        PolicyBundleTransactionRequestDto AddPolicyBundleCancellation(PolicyBundleTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Endorsement
        PolicyTransactionsResponseDto GetPolicyEndorsements(SecurityContext securityContext, AuditContext auditContext);
        PolicyTransactionRequestDto AddPolicyEndorsement(PolicyTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        PolicyTransactionResponseDto GetPolicyEndorsementById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Endorsement Approval
        bool ApprovePolicyEndorsement(Guid Id, bool Status, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region History
        PolicyHistoryRequestDto AddPolicyHistory(PolicyHistoryRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        PolicyHistoriesResponseDto GetPolicyHistories(SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Cancelation
        PolicyTransactionRequestDto AddPolicyCancellation(PolicyTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        #region Inquiry
        PolicyTransactionRequestDto GetPolicyInquiryData(
            SecurityContext securityContext,
            AuditContext auditContext);
        #endregion

        PolicyTransactionTypesResponseDto GetPolicyTransactionTypes(SecurityContext securityContext, AuditContext auditContext);

        object GetPoliciesByBordxIdForGrid(GetPoliciesByBordxIdRequestDto GetPoliciesByBordxIdRequestDto, SecurityContext securityContext, AuditContext auditContext);

        BordxExportResponseDto ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetPoliciesByBordxIdForViewGrid(BordxViewGridRequestDto BordxViewGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetPoliciesForSearchGrid(PolicySearchGridRequestDto PolicySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);
        List<PolicyResponseDto> GetPolicysByIdAndType(Guid guid, string v, SecurityContext context1, AuditContext context2);
        object GetPoliciesForSearchGridReneval(PolicySearchGridRequestDto PolicySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetPoliciesForSearchGridInquiry(PolicySearchInquiryGridRequestDto PolicySearchInquiryGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string SavePolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext);

        string UpdatePolicyV2(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext);

        SerialNumberCheckResponseDto SerialNumberCheck(string SerialNumber, string CommodityCode, Guid LoggedInUserId, Guid DealerId, SecurityContext securityContext, AuditContext auditContext);

        CustomerCheckResponseDto CheckCustomerById(Guid CustomerId, Guid LoggedInUserId, SecurityContext securityContext, AuditContext auditContext);

        object GetDocumentTypesByPageName(string PageName, SecurityContext securityContext, AuditContext auditContext);

        AttachmentsResponseDto GetAttachmentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);
        object GetClaimHistorysByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext);

        EligibilityCheckResponse EligibilityCheckRequest(EligibilityCheckRequest eligibilityCheckRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetDealerAccessByUserId(Guid LoggedInUserId, SecurityContext securityContext, AuditContext auditContext);

        string EndorsePolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetAllPolicyEndorsementDetailsForApproval(Guid BundlePolicyId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllPolicyInquiryDetails(Guid BundlePolicyId, SecurityContext securityContext, AuditContext auditContext);
        object GetPolicyById2(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);


        string RejectEndorsement(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        string ApproveEndorsement(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        string PolicyCancellation(Guid PolicyBundleId, string CancellationComment, Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        string GetPolicyCancellationCommentByPolicyBundleId(Guid PolicyBundleId, SecurityContext securityContext, AuditContext auditContext);

        string PolicyCancellationApproval(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        string PolicyCancellationReject(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        string TransferPolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetPolicyTransferHistoryById(Guid policyBundleId, SecurityContext securityContext, AuditContext auditContext);

        object GetPremiumBreakdown(PremiumBreakdownRequestDto premiumBreakdownRequest, SecurityContext securityContext, AuditContext auditContext);

        string RenewPolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext);

        byte[] GetPolicyAttachmentById(Guid policyid, SecurityContext securityContext, AuditContext auditContext);
        object RetrivePolicySectionData(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext);
        string GetPolicyNumber(Guid branchId, Guid dealerId, Guid productId,Guid tpaId,Guid commodityTypeId, SecurityContext securityContext, AuditContext auditContext);
        object ManufacturerWarrentyAvailabilityCheckOnPolicySave(ManufacturerWarrentyAvailabilityCheckDto policyDetails, SecurityContext securityContext, AuditContext auditContext);

        object GetPolicesByCustomerId(Guid CustomerId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAdditionalFieldDetailsByProductCode(Guid tpaId, string productCode);
        object GetTireDetailsByInvoiceCode(Guid tpaId, string invoiceCode);
        object SaveCustomerEnterdInvoiceDetails(SaveCustomerPolicyInfoRequestDto saveCustomerPolicyDto);
        object LoadSavedCustomerInvoiceDataById(Guid tpaId, Guid tempInvoiceId);
        object GetAllCountries(Guid tpaId);
        object GetAllCitiesByCountryId(Guid tpaId, Guid countryId);
        object GetAllUsageTypesByTpaId(Guid tpaId);
        object GetAllCustomerTypes(Guid tpaId);
        object SaveCustomerEnterdPolicy(SaveCustomerEnterdPolicyRequestDto saveCustomerPolicyDto);

        object GetOtherTirePolicyById(Guid PolicyById, SecurityContext securityContext, AuditContext auditContext);

       // InvoiceCodeRequestDto UpdateInvoiceCode(InvoiceCodeRequestDto IC, SecurityContext securityContext, AuditContext auditContext);

        //InvoiceCodeRequestDto UpdateInvoiceCode(InvoiceCodeRequestDto IC, PolicyHistoryRequestDto Ph, SecurityContext securityContext, AuditContext auditContext);

        InvoiceCodeRequestDto UpdateInvoiceCode(InvoiceCodeRequestDto IC, PolicyRequestDto P, SecurityContext securityContext, AuditContext auditContext);
        object DownloadPolicyStatementforTYER(Guid policyid, SecurityContext securityContext, AuditContext auditContext);
        object GetEMIValue(decimal loneAmount,Guid ContractId, SecurityContext securityContext, AuditContext auditContext);
        object checkCustomerExist(String mobile, SecurityContext securityContext, AuditContext auditContext);
        bool addPolicyAttachement(Guid id, List<Guid> uploadAttachments, SecurityContext context1, AuditContext context2);
        AttachmentsByUsersResponseDto GetAttachmentsByPolicyIdByUserType(Guid policyId, SecurityContext context1, AuditContext context2);
    }
}
