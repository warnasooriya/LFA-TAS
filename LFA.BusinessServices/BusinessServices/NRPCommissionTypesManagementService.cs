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
    internal sealed class NRPCommissionTypesManagementService : INRPCommissionTypesManagementService
    {

        public NRPCommissionTypessResponseDto GetNRPCommissionTypess(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            NRPCommissionTypessResponseDto result = null;

            NRPCommissionTypessRetrievalUnitOfWork uow = new NRPCommissionTypessRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public NRPCommissionTypesRequestDto AddNRPCommissionTypes(
            NRPCommissionTypesRequestDto NRPCommissionTypes, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                NRPCommissionTypesRequestDto result = new NRPCommissionTypesRequestDto();
                NRPCommissionTypesInsertionUnitOfWork uow = new NRPCommissionTypesInsertionUnitOfWork(NRPCommissionTypes);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.NRPCommissionTypesInsertion = uow.NRPCommissionTypes.NRPCommissionTypesInsertion;
                return result;
        }

        public NRPCommissionTypesResponseDto GetNRPCommissionTypesById(
            Guid NRPCommissionTypesId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            NRPCommissionTypesResponseDto result = new NRPCommissionTypesResponseDto();

            NRPCommissionTypesRetrievalUnitOfWork uow = new NRPCommissionTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.NRPCommissionTypesId = NRPCommissionTypesId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public NRPCommissionTypesRequestDto UpdateNRPCommissionTypes(
            NRPCommissionTypesRequestDto NRPCommissionTypes, 
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            NRPCommissionTypesRequestDto result = new NRPCommissionTypesRequestDto();
            NRPCommissionTypesUpdationUnitOfWork uow = new NRPCommissionTypesUpdationUnitOfWork(NRPCommissionTypes);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.NRPCommissionTypesInsertion = uow.NRPCommissionTypes.NRPCommissionTypesInsertion;
            return result;
        }

        public NRPCommissionContractMappingsResponseDto GetNRPCommissionContractMappings(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            NRPCommissionContractMappingsResponseDto result = null;

            NRPCommissionContractMappingsRetrievalUnitOfWork uow = new NRPCommissionContractMappingsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public NRPCommissionContractMappingRequestDto AddNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping, SecurityContext securityContext,
            AuditContext auditContext)
        {
            NRPCommissionContractMappingRequestDto result = new NRPCommissionContractMappingRequestDto();
            NRPCommissionContractMappingInsertionUnitOfWork uow = new NRPCommissionContractMappingInsertionUnitOfWork(NRPCommissionContractMapping);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.NRPCommissionContractMappingInsertion = uow.NRPCommissionContractMapping.NRPCommissionContractMappingInsertion;
            return result;
        }


        public NRPCommissionContractMappingResponseDto GetNRPCommissionContractMappingById(Guid NRPCommissionContractMappingId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            NRPCommissionContractMappingResponseDto result = new NRPCommissionContractMappingResponseDto();

            NRPCommissionContractMappingRetrievalUnitOfWork uow = new NRPCommissionContractMappingRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.NRPCommissionContractMappingId = NRPCommissionContractMappingId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public NRPCommissionContractMappingRequestDto UpdateNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping, SecurityContext securityContext,
           AuditContext auditContext)
        {
            NRPCommissionContractMappingRequestDto result = new NRPCommissionContractMappingRequestDto();
            NRPCommissionContractMappingUpdationUnitOfWork uow = new NRPCommissionContractMappingUpdationUnitOfWork(NRPCommissionContractMapping);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.NRPCommissionContractMappingInsertion = uow.NRPCommissionContractMapping.NRPCommissionContractMappingInsertion;
            return result;
        }

       
       
    }
}
