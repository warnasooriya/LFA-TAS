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
    internal sealed class TransmissionTypeManagementService : ITransmissionTypeManagementService
    {

        public TransmissionTypesResponseDto GetTransmissionTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            TransmissionTypesResponseDto result = null;

            TransmissionTypesRetrievalUnitOfWork uow = new TransmissionTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public TransmissionTechnologiesResponseDto GetTransmissionTechnologies(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            TransmissionTechnologiesResponseDto result = null;

            TransmissionTechnologiesRetrievalUnitOfWork uow = new TransmissionTechnologiesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public TransmissionTypeRequestDto AddTransmissionType(TransmissionTypeRequestDto TransmissionType, SecurityContext securityContext,
            AuditContext auditContext) {
                TransmissionTypeRequestDto result = new TransmissionTypeRequestDto();
                TransmissionTypeInsertionUnitOfWork uow = new TransmissionTypeInsertionUnitOfWork(TransmissionType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.TransmissionTypeInsertion = uow.Transmission.TransmissionTypeInsertion;
                return result;
        }


        public TransmissionTypeResponseDto GetTransmissionTypeById(Guid TransmissionTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            TransmissionTypeResponseDto result = new TransmissionTypeResponseDto();

            TransmissionTypeRetrievalUnitOfWork uow = new TransmissionTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.TransmissionTypeId = TransmissionTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public TransmissionTypeRequestDto UpdateTransmissionType(TransmissionTypeRequestDto TransmissionType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            TransmissionTypeRequestDto result = new TransmissionTypeRequestDto();
            TransmissionTypeUpdationUnitOfWork uow = new TransmissionTypeUpdationUnitOfWork(TransmissionType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.TransmissionTypeInsertion = uow.TransmissionType.TransmissionTypeInsertion;
            return result;
        }

       
       
    }
}
