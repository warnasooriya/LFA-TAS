using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ITPAManagementService
    {
        TPAsResponseDto GetAllTPAs(SecurityContext securityContext, AuditContext auditContext);

        TPAsResponseDto GetTPADetailsByTPAId(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        bool SaveTPA(DataTransfer.Requests.TPARequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        bool UpdateTPA(DataTransfer.Requests.TPARequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        bool checkIsIprestrcted(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, string userIp);
    }
}
