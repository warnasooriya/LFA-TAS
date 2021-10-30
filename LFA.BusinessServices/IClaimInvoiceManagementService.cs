using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IClaimInvoiceManagementService
    {
       ClaimInvoiceEntryRequestDto AddClaimInvoice(ClaimInvoiceEntryRequestDto ClaimInvoiceEntry, SecurityContext securityContext, AuditContext auditContext);

       object GetAllClaimByDealerId(Guid ClaimSubmittedDealerId,string claimNumber ,SecurityContext securityContext, AuditContext auditContext);

       object GetAllClaimInvoiceEntryForSearchGrid(ClaimInvoiceEntrySearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

       ClaimInvoiceResponseDto GetClaimInvoiceEntryById(Guid ClaimInvoiceEntryId, SecurityContext securityContext, AuditContext auditContext);

       ClaimInvoiceEntryRequestDto UpdateClaimInvoice(ClaimInvoiceEntryRequestDto ClaimInvoice, SecurityContext securityContext, AuditContext auditContext);

       object GetAllClaimInvoiceEntryClaimForSearchGrid(ClaimInvoiceEntryClaimSearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

       object GetAllClaimBatchGroupById(Guid ClaimBatchGroupId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllSubmittedInvoiceClaimByDealerId(Guid claimSubmittedDealerId, string invoiceNumber,string  claimNumber, SecurityContext securityContext, AuditContext auditContext);
        object GetAllClaimPartDetailsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext);
        object AddAjusments(Guid claimId, decimal adjustPartAmount, decimal adjustLabourAmount, decimal adjustSundryAmount, SecurityContext context1, AuditContext context2);
        object RetriveInvoiceEntryData(Guid claimSubmittedDealerId, SecurityContext context1, AuditContext context2);
        ClaimInvoiceEntryRequestDto ConfirmClaimInvoice(ClaimInvoiceEntryRequestDto claimInvoice, SecurityContext context1, AuditContext context2);
    }
}
