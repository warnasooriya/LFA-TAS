using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class CylinderCountManagementService : ICylinderCountManagementService
    {

        public CylinderCountsResponseDto GetCylinderCounts(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            CylinderCountsResponseDto result = null;

            CylinderCountsRetrievalUnitOfWork uow = new CylinderCountsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CylinderCountsResponseDto GetParentCylinderCounts(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CylinderCountsResponseDto result = null;

            CylinderCountsRetrievalUnitOfWork uow = new CylinderCountsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CylinderCountRequestDto AddCylinderCount(CylinderCountRequestDto CylinderCount, SecurityContext securityContext,
            AuditContext auditContext) {
                CylinderCountRequestDto result = new CylinderCountRequestDto();
                CylinderCountInsertionUnitOfWork uow = new CylinderCountInsertionUnitOfWork(CylinderCount);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.CylinderCountInsertion = uow.CylinderCount.CylinderCountInsertion;
                return result;
        }


        public CylinderCountResponseDto GetCylinderCountById(Guid CylinderCountId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CylinderCountResponseDto result = new CylinderCountResponseDto();

            CylinderCountRetrievalUnitOfWork uow = new CylinderCountRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.cylinderCountId = CylinderCountId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CylinderCountRequestDto UpdateCylinderCount(CylinderCountRequestDto CylinderCount, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CylinderCountRequestDto result = new CylinderCountRequestDto();
            CylinderCountUpdationUnitOfWork uow = new CylinderCountUpdationUnitOfWork(CylinderCount);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CylinderCountInsertion = uow.CylinderCount.CylinderCountInsertion;
            return result;
        }

        
       
    }
}
