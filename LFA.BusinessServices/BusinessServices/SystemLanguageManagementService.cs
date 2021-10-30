
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class SystemLanguageManagementService : ISystemLanguageManagementService
    {
        public List<SystemLanguageResponseDto> GetAllLanguages(SecurityContext securityContext, AuditContext auditContext)
        {
            List<SystemLanguageResponseDto> result = null;
            SystemLanguageGetAllUnitOfWork uow = new SystemLanguageGetAllUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.result;
            return result;
        }
    }
}
