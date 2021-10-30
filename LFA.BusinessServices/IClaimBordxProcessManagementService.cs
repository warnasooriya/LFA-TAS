using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IClaimBordxProcessManagementService
    {
        List<int> GetClaimBordxYears(SecurityContext securityContext, AuditContext auditContext);

        object GetClaimBordxByYearAndMonth(int Year, int Month, Guid Insurerid, Guid Reinsurerid, SecurityContext securityContext, AuditContext auditContext);

        object ProcessClaimBordxForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        bool ClaimBordxProcess(Guid ClaimbordxId, Guid UserId, bool IsProcess, SecurityContext securityContext, AuditContext auditContext);

        bool ClaimBordxProcessUpdate(Guid ClaimbordxprocessId, Guid UserId, bool IsConfirm, SecurityContext securityContext, AuditContext auditContext);

        object ClaimBordxProcessedDetailsByYear(int Year, Guid Insurerid, Guid Reinsurerid, SecurityContext securityContext, AuditContext auditContext);

        bool ClaimBordxCanConfirmedByYear(int year, Guid insurerid, Guid reinsurerid, SecurityContext securityContext, AuditContext auditContext);


        List<int> GetBordxYearsByClaim(SecurityContext securityContext, AuditContext auditContext);

        ClaimBordxExportResponseDto GetProcessClaimBordxExport(Guid bordxId, SecurityContext securityContext, AuditContext auditContext);
    }
}
