using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ITPABranchManagementService
    {

        TPABranchesResponseDto GetTPABranchesByTPAId(Guid TPAId, SecurityContext securityContext, AuditContext auditContext);

        TPABranchesResponseDto GetTPABranches(SecurityContext securityContext, AuditContext auditContext);

        bool SaveTPABranch(DataTransfer.Requests.TPABranchRequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        bool UpdateTPABranch(DataTransfer.Requests.TPABranchRequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        TPABranchesResponseDto GetTPABranchesBySystemUserId(Guid loggedUserId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllTimezones(SecurityContext securityContext, AuditContext auditContext);

        object GetTimeZoneById(Guid TimeZoneId, SecurityContext securityContext, AuditContext auditContext);

        //TimeZoneResponseDto GetAllTimeZonesTypesByTimeZonesId(Guid Id, SecurityContext securityContext, AuditContext auditContext);

        bool IsExsistingTpaByCode(Guid Id, string branchCode, SecurityContext securityContext, AuditContext auditContext);
    }
          
}
