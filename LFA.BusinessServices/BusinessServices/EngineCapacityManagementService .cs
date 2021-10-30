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
    internal sealed class EngineCapacityManagementService : IEngineCapacityManagementService
    {

        public EngineCapacitiesResponseDto GetEngineCapacities(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            EngineCapacitiesResponseDto result = null;

            EngineCapacitiesRetrievalUnitOfWork uow = new EngineCapacitiesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

       

        public EngineCapacityRequestDto AddEngineCapacity(EngineCapacityRequestDto EngineCapacity, SecurityContext securityContext,
            AuditContext auditContext) {
                EngineCapacityRequestDto result = new EngineCapacityRequestDto();
                EngineCapacityInsertionUnitOfWork uow = new EngineCapacityInsertionUnitOfWork(EngineCapacity);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.EngineCapacityInsertion = uow.EngineCapacity.EngineCapacityInsertion;
                return result;
        }


        public EngineCapacityResponseDto GetEngineCapacityById(Guid EngineCapacityId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            EngineCapacityResponseDto result = new EngineCapacityResponseDto();

            EngineCapacityRetrievalUnitOfWork uow = new EngineCapacityRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.engineCapacityId = EngineCapacityId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public EngineCapacityRequestDto UpdateEngineCapacity(EngineCapacityRequestDto EngineCapacity, SecurityContext securityContext,
           AuditContext auditContext)
        {
            EngineCapacityRequestDto result = new EngineCapacityRequestDto();
            EngineCapacityUpdationUnitOfWork uow = new EngineCapacityUpdationUnitOfWork(EngineCapacity);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.EngineCapacityInsertion = uow.EngineCapacity.EngineCapacityInsertion;
            return result;
        }

        public bool IsExsistingEngineCapacityByEngineCapacityNumber(Guid Id, decimal engineCapacityNumber,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool Result = false;
            IsExsistingEngineCapacityByEngineCapacityNumberUnitOfWorks uow = new IsExsistingEngineCapacityByEngineCapacityNumberUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.EngineCapacityNumber = engineCapacityNumber;
            uow.MesureType = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }

   
       
    }
}
