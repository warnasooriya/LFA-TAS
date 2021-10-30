using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IDealerManagementService
    {
        DealersRespondDto GetAllDealers(
            SecurityContext securityContext,
            AuditContext auditContext);
        DealersRespondDto GetAllDealersByCountry(
            Guid countryId,
    SecurityContext securityContext,
    AuditContext auditContext);

        string AddDealer(DealerRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);

        string AddDealerComment(AddDealerCommentRequestDto dealerComment,
           SecurityContext securityContext,
           AuditContext auditContext);

        DealerRespondDto GetDealerById(Guid DealerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        string UpdateDealer(DealerRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealerStaffsResponseDto GetDealerStaffs(
            SecurityContext securityContext,
            AuditContext auditContext);

        DealerStaffAddResponse AddDealerStaff(List<DealerStaffRequestDto> DealerStaff, List<DealerBranchRequestDto> DealerBranch,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealerStaffResponseDto GetDealerStaffById(Guid DealerStaffId,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealerStaffRequestDto UpdateDealerStaff(DealerStaffRequestDto DealerStaff, bool Enable,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealersRespondDto GetAllDealersByUserId(Guid UserId, SecurityContext securityContext, AuditContext auditContext);

        object GetYearsMonthsForDealerInvoices(SecurityContext securityContext, AuditContext auditContext);



        object GetDealerStaffByDealerIdandBranchId(Guid DealerId, Guid BranchId, SecurityContext securityContext, AuditContext auditContext);
        object GetDealersByBordx(Guid bordxId, SecurityContext securityContext, AuditContext auditContext);
    }
}
