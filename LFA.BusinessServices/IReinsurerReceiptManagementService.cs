using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IReinsurerReceiptManagementService
    {
        ClaimBordxsResponseDto GetBordxs(
            SecurityContext securityContext,
            AuditContext auditContext);

        ClaimBordxsResponseDto GetBordxsById(Guid ReId, Guid InId, int Year,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxPaymentRequestDto AddBordxPayment(BordxPaymentRequestDto BordxPayment,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxPaymentsResponseDto GetClaimBordxPaymentsById(Guid ClaimBordxId,
           SecurityContext securityContext,
           AuditContext auditContext);

        BordxPaymentRequestDto UpdateBordxPayment(BordxPaymentRequestDto BordxPayment,
           SecurityContext securityContext,
           AuditContext auditContext);

        
    }
}
