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
    internal sealed class DealTypeManagementService : IDealTypeManagementService
    {

        public DealTypesResponseDto GetDealTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            DealTypesResponseDto result = null;

            DealTypesRetrievalUnitOfWork uow = new DealTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public DealTypeRequestDto AddDealType(DealTypeRequestDto DealType, SecurityContext securityContext,
            AuditContext auditContext) {
                DealTypeRequestDto result = new DealTypeRequestDto();
                DealTypeInsertionUnitOfWork uow = new DealTypeInsertionUnitOfWork(DealType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.DealTypeInsertion = uow.DealType.DealTypeInsertion;
                return result;
        }


        public DealTypeResponseDto GetDealTypeById(Guid DealTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            DealTypeResponseDto result = new DealTypeResponseDto();

            DealTypeRetrievalUnitOfWork uow = new DealTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.DealTypeId = DealTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealTypeRequestDto UpdateDealType(DealTypeRequestDto DealType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            DealTypeRequestDto result = new DealTypeRequestDto();
            DealTypeUpdationUnitOfWork uow = new DealTypeUpdationUnitOfWork(DealType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.DealTypeInsertion = uow.DealType.DealTypeInsertion;
            return result;
        }

       
       
    }
}
