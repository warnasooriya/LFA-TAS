using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IClaimBordxManagementService
    {
        ClaimBordxsResponseDto GetBordxs(
            SecurityContext securityContext,
            AuditContext auditContext);

        string AddClaimBordx(ClaimBordxRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext);

        string GetNextBordxNumber(int year, int month, Guid insurerId, Guid ReinsurerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        string DeleteClaimBordx(Guid ClaimBordxId, SecurityContext securityContext, AuditContext auditContext);

        ClaimBordxsYearsResponseDto GetClaimBordxYears(
            SecurityContext securityContext,
            AuditContext auditContext);

        ClaimBordxsYearsResponseDto GetClaimBordxYearsForProcess(
            SecurityContext securityContext,
            AuditContext auditContext);
        //ClaimBordxResponseDto GetBordxById(Guid BordxId,
        //    SecurityContext securityContext,
        //    AuditContext auditContext);

        //ClaimBordxResponseDto UpdateBordx(ClaimBordxRequestDto Bordx,
        //    SecurityContext securityContext,
        //    AuditContext auditContext);


        object ConfirmedClaimBordxYears(SecurityContext securityContext, AuditContext auditContext);

        object GetConfirmedClaimBordxForGrid(ConfirmedClaimBordxForGridRequestDto ConfirmedClaimBordxForGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimBordxByDateForGrid(GetAllClaimBordxByDateRequestDto GetAllClaimBordxByDateRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string ClaimBordxReopen(ClaimBordxReopenRequestDto ClaimBordxReopenRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetConfirmedClaimBordxYearlybyYear(SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimBordxByYearandCountryForGrid(GetAllClaimBordxByYearandCountryRequestDto GetAllClaimBordxByYearandCountryRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string ClaimBordxYealyReopen(ClaimBordxYearlyReopenRequestDto ClaimBordxYearlyReopenRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object GetClaimBordxBordxNumbers(SecurityContext securityContext, AuditContext auditContext);

        string ClaimBordxYearProcess(int year,Guid Reinsurer,Guid Insurer, SecurityContext securityContext, AuditContext auditContext);

        string ClaimBordxYearProcessConfirm(int year, Guid Reinsurer, Guid Insurer, SecurityContext securityContext, AuditContext auditContext);

    }
}
