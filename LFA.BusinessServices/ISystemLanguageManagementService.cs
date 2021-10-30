using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface ISystemLanguageManagementService
    {
        List<SystemLanguageResponseDto> GetAllLanguages(SecurityContext securityContext,AuditContext auditContext);
    }
}
