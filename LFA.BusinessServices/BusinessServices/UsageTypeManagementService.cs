using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class UsageTypeManagementService : IUsageTypeManagementService
    {
        public UsageTypesResponseDto GetAllUsageTypes(
            SecurityContext securityContext, 
            AuditContext auditContext) 
        {
            UsageTypesResponseDto result = null;
            UsageTypeRetrievalUnitOfWork uow = new UsageTypeRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        
        }
    }
}
