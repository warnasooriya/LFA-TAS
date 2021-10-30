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
    internal sealed class IdTypeManagementService : IIdTypeManagementService
    {
        public IdTypesResponseDto GetAllIdTypes(
            SecurityContext securityContext, 
            AuditContext auditContext) 
        {
            IdTypesResponseDto result = null;
            IdTypeRetrievalUnitOfWork uow = new IdTypeRetrievalUnitOfWork();
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
