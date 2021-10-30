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
    internal sealed class NationalityManagementService : INationalityManagementService
    {
        public NationalitiesResponseDto GetAllNationalities(
            SecurityContext securityContext, 
            AuditContext auditContext) 
        {
            NationalitiesResponseDto result = null;
            NationalityRetrievalUnitOfWork uow = new NationalityRetrievalUnitOfWork();
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
